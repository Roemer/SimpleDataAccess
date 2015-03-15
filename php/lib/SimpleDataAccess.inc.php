<?php

/**
 * SimplSDAtaAccess.inc.php
 * Provides a simple Data Access Layer for PHP
 * 
 * Project Url: https://github.com/Roemer/SimplSDAtaAccess
 * 
 * @author Roman Baeriswyl
 * @version 0.9
 * @copyright Copyright &copy; 2012-2015 Roman Baeriswyl
 * 
 * ToDo
 * ----------
 * Add ExcludeFields to Query
 * Add IncludeFields to Query
 */

/**
 * This Class provides Connections
 */
class SDAConnectionProvider {

    private $_dbUser;
    private $_dbPassword;
    private $_dbName;

    function __construct($dbUser, $dbPassword, $dbName) {
        $this->_dbUser = $dbUser;
        $this->_dbPassword = $dbPassword;
        $this->_dbName = $dbName;
    }

    // Gets the PDO Object
    public function GetDBConnection() {
        try {
            $pdo = new PDO("mysql:host=localhost;dbname=" . $this->_dbName, $this->_dbUser, $this->_dbPassword, array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8"));
        } catch (PDOException $e) {
            die('SQL-Connection Error: ' . $e->getMessage());
        }
        return $pdo;
    }

}

/**
 * This Class is for creating Queries
 */
class SDAQuery {

    public $Limit;
    public $ExpressionList;
    public $OrderFields;
    public $ExcludeFields;

    function __construct() {
        $this->Limit = 0;
        $this->ExpressionList = array();
        $this->OrderFields = array();
        $this->ExcludeFields = array();
    }

    function Add(SDABaseCriteria $expression) {
        $this->ExpressionList[] = $expression;
        return $this;
    }

    function AddOrder($fieldNumber, $sortDirection) {
        $this->OrderFields[] = new SDAOrderByItem($fieldNumber, $sortDirection);
        return $this;
    }

    function AddExcludeField($fieldNumber) {
        $this->ExcludeFields[] = $fieldNumber;
    }

    function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        $firstFlag = true;
        $str = '';
        foreach ($this->ExpressionList as $expression) {
            // Check if the current Expression is a Junction Criteria (And/Or)
            if (is_subclass_of($expression, 'SDAJunctionCriteria')) {
                if (!count($expression->ExpressionList) > 0) {
                    // It doesn't have and Sub-Criterias so skip it
                    continue;
                }
            }
            if (!$firstFlag) {
                $str .=' AND ';
            }
            $str .= $expression->GenerateAndFill($mappingInfo, $paramList);
            $firstFlag = false;
        }
        return $str;
    }

}

/**
 * This Class is used for the Parameter Values in a Query
 */
class SDAFieldParameter {

    public $ParamName;
    public $ParamType;
    public $Value;

    function __construct($paramName, $paramType, $value) {
        $this->ParamName = $paramName;
        $this->ParamType = $paramType;
        $this->Value = $value;
    }

}

/**
 * This Class holds Information for Ordering
 */
class SDAOrderByItem {

    public $FieldNumber;
    public $SortDirection;

    function __construct($fieldNumber, $sortDirection) {
        $this->FieldNumber = $fieldNumber;
        $this->SortDirection = $sortDirection;
    }

    function Generate(SDAMappingInfo $mappingInfo) {
        return SDAUtils::Escape($mappingInfo->GetField($this->FieldNumber)->FieldName) . ' ' . (($this->SortDirection == 1) ? 'ASC' : 'DESC');
    }

}

/**
 * Simulated Enum for the Sorting-Direction
 */
class SDASortDirection {

    const ASC = 1;
    const DESC = 2;

}

/**
 * This is the Base-Class used for the Querying
 */
abstract class SDABaseCriteria {

    public $IsNegate = false;

    public function Negate() {
        $this->IsNegate = true;
        return $this;
    }

    protected function Finalize($text) {
        return $this->IsNegate ? "NOT ($text)" : $text;
    }

    public abstract function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList);
}

/**
 * This is the Base-Class for Junction Criterias
 */
abstract class SDAJunctionCriteria extends SDABaseCriteria {

    public $ExpressionList;

    function __construct() {
        $this->ExpressionList = array();
    }

    public function Add(SDABaseCriteria $expression) {
        $this->ExpressionList[] = $expression;
        return $this;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        if (count($this->ExpressionList) > 0) {
            $str = '(';
            $flag = false;
            foreach ($this->ExpressionList as $expression) {
                if ($flag) {
                    $str .= sprintf(' %s ', $this->GetOperator());
                }
                $str .= $expression->GenerateAndFill($mappingInfo, $paramList);
                $flag = true;
            }
            $str .= ')';
            return $this->Finalize($str);
        }
        return '';
    }

    public abstract function GetOperator();
}

/**
 * This is the Base-Class for Field Criterias
 */
abstract class SDAFieldCriteria extends SDABaseCriteria {

    public $FieldNumber;
    public $Value;

    function __construct($fieldNumber, $value) {
        $this->FieldNumber = $fieldNumber;
        $this->Value = $value;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        $field = $mappingInfo->GetField($this->FieldNumber);
        $paramName = ':' . $field->FieldName . '_' . count($paramList);
        $paramList[] = new SDAFieldParameter($paramName, $field->FieldType, $this->Value);
        return $this->Finalize(sprintf("%s %s %s", SDAUtils::Escape($field->FieldName), $this->GetOperator(), $paramName));
    }

    public abstract function GetOperator();
}

/**
 * Equal Criteria
 */
class SDAEq extends SDAFieldCriteria {

    function __construct($fieldNumber, $value) {
        parent::__construct($fieldNumber, $value);
    }

    public function GetOperator() {
        return '=';
    }

}

/**
 * Greater-Equal Criteria
 */
class SDAGe extends SDAFieldCriteria {

    function __construct($fieldNumber, $value) {
        parent::__construct($fieldNumber, $value);
    }

    public function GetOperator() {
        return '>=';
    }

}

/**
 * Greater-Than Criteria
 */
class SDAGt extends SDAFieldCriteria {

    function __construct($fieldNumber, $value) {
        parent::__construct($fieldNumber, $value);
    }

    public function GetOperator() {
        return '>';
    }

}

/**
 * Less-Equal Criteria
 */
class SDALe extends SDAFieldCriteria {

    function __construct($fieldNumber, $value) {
        parent::__construct($fieldNumber, $value);
    }

    public function GetOperator() {
        return '<=';
    }

}

/**
 * Less-Than Criteria
 */
class SDALt extends SDAFieldCriteria {

    function __construct($fieldNumber, $value) {
        parent::__construct($fieldNumber, $value);
    }

    public function GetOperator() {
        return '<';
    }

}

/**
 * And Junction Criteria
 */
class SDAAnd extends SDAJunctionCriteria {

    public function GetOperator() {
        return "AND";
    }

}

/**
 * Or Junction Criteria
 */
class SDAOr extends SDAJunctionCriteria {

    public function GetOperator() {
        return "OR";
    }

}

/**
 * IsNull Criteria
 */
class SDAIsNull extends SDABaseCriteria {

    public $FieldNumber;

    function __construct($fieldNumber) {
        $this->FieldNumber = $fieldNumber;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        $field = $mappingInfo->GetField($this->FieldNumber);
        return $this->Finalize(sprintf("%s IS NULL", SDAUtils::Escape($field->FieldName)));
    }

}

/**
 * Like Criteria
 */
class SDALike extends SDABaseCriteria {

    public $FieldNumber;
    public $Value;

    function __construct($fieldNumber, $value) {
        $this->FieldNumber = $fieldNumber;
        $this->Value = $value;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        $field = $mappingInfo->GetField($this->FieldNumber);
        $paramName = ':' . $field->FieldName . '_' . count($paramList);
        $paramList[] = new SDAFieldParameter($paramName, $field->FieldType, $this->Value);
        return $this->Finalize(sprintf("%s LIKE %s", SDAUtils::Escape($field->FieldName), $paramName));
    }

}

/**
 * Between Criteria
 */
class SDABetween extends SDABaseCriteria {

    public $FieldNumber;
    public $ValueLower;
    public $ValueHigher;

    function __construct($fieldNumber, $valueLower, $valueHigher) {
        $this->FieldNumber = $fieldNumber;
        $this->ValueLower = $valueLower;
        $this->ValueHigher = $valueHigher;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        $field = $mappingInfo->GetField($this->FieldNumber);
        $paramNameLower = ':' . $field->FieldName . '_min_' . count($paramList);
        $paramNameHigher = ':' . $field->FieldName . '_max_' . count($paramList);
        $paramList[] = new SDAFieldParameter($paramNameLower, $field->FieldType, $this->ValueLower);
        $paramList[] = new SDAFieldParameter($paramNameHigher, $field->FieldType, $this->ValueHigher);
        return $this->Finalize(sprintf("%s BETWEEN %s AND %s", SDAUtils::Escape($field->FieldName), $paramNameLower, $paramNameHigher));
    }

}

/**
 * In Criteria
 */
class SDAIn extends SDABaseCriteria {

    public $FieldNumber;
    public $ValueList;

    function __construct($fieldNumber, array $valueList) {
        $this->FieldNumber = $fieldNumber;
        $this->ValueList = $valueList;
    }

    public function GenerateAndFill(SDAMappingInfo $mappingInfo, array &$paramList) {
        if ($this->ValueList != null && count($this->ValueList) > 0) {
            $field = $mappingInfo->GetField($this->FieldNumber);
            $str = sprintf("%s IN (", SDAUtils::Escape($field->FieldName));
            $firstFlag = true;
            foreach ($this->ValueList as $value) {
                if (!$firstFlag) {
                    $str .= ', ';
                }
                $paramName = ':' . $field->FieldName . '_' . count($paramList);
                $paramList[] = new SDAFieldParameter($paramName, $field->FieldType, $value);
                $str .= $paramName;
                $firstFlag = false;
            }
            $str .= ')';
            return $this->Finalize($str);
        }
        return '';
    }

}

/**
 * This Class holds the Mapping Info for the Table and all Fields
 */
class SDAMappingInfo {

    public $TableName;
    public $FieldList;

    function __construct($tableName) {
        $this->TableName = $tableName;
        $this->FieldList = array();
    }

    public function AddField($fieldNumber, $fieldName, $fieldType, $isPrimary = false, $isAutoIncrement = false) {
        $this->FieldList[$fieldNumber] = new SDAMappingInfoField($fieldNumber, $fieldName, $fieldType, $isPrimary, $isAutoIncrement);
    }

    public function GetField($fieldNumber) {
        return $this->FieldList[$fieldNumber];
    }

}

/**
 *  This Class holds the Mapping Info for a Field
 */
class SDAMappingInfoField {

    public $FieldNumber;
    public $FieldName;
    public $FieldType;
    public $IsPrimary;
    public $IsAutoIncrement;

    function __construct($fieldNumber, $fieldName, $fieldType, $isPrimary, $isAutoIncrement) {
        $this->FieldNumber = $fieldNumber;
        $this->FieldName = $fieldName;
        $this->FieldType = $fieldType;
        $this->IsPrimary = $isPrimary;
        $this->IsAutoIncrement = $isAutoIncrement;
    }

}

/**
 * This Class is used for the Batch Updates
 */
class SDAUpdateField {

    public $FieldNumber;
    public $NewValue;

    function __construct($fieldNumber, $newValue) {
        $this->FieldNumber = $fieldNumber;
        $this->NewValue = $newValue;
    }

}

/**
 * Class with different Utilities used
 */
class SDAUtils {

    public static function Escape($string) {
        return '`' . $string . '`';
    }

    public static function FormatSDAte(DateTime $date = null) {
        if ($date == null) {
            $date = new DateTime();
        }
        return $date->format('Y-m-d H:i:s');
    }

}

/**
 * Base Class for the Entities
 */
abstract class SDAEntityBase {

    public $IsNew = true;

}

/**
 * Core-Class which is for Executing all the Queries
 */
class SDAEntityHandler {

    public $DEBUG = false;
    /* @var $_connectionProvider SDAConnectionProvider */
    private $_connectionProvider = null;

    function __construct(SDAConnectionProvider $connectionProvider) {
        $this->_connectionProvider = $connectionProvider;
    }

    public function Save(SDAEntityBase $entity, array $excludeFields = null) {
        if ($entity->IsNew) {
            $this->Insert($entity);
        } else {
            $this->Update($entity, $excludeFields);
        }
    }

    private function Insert(SDAEntityBase $entity) {
        $mapping = self::GetMappingFromEntity($entity);
        $autoIncField = null;
        // Build the Insert Command
        $cmd = 'INSERT INTO ' . SDAUtils::Escape($mapping->TableName);
        $cmd .= ' (';
        foreach ($mapping->FieldList as $field) {
            if ($field->IsAutoIncrement) {
                $autoIncField = $field;
                continue;
            }
            $cmd .= SDAUtils::Escape($field->FieldName) . ', ';
        }
        $cmd = substr($cmd, 0, -2);
        $cmd .= ') VALUES(';
        foreach ($mapping->FieldList as $field) {
            if ($field->IsAutoIncrement) {
                continue;
            }
            $cmd .= ':' . $field->FieldName . ', ';
        }
        $cmd = substr($cmd, 0, -2);
        $cmd .= ')';
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        foreach ($mapping->FieldList as $field) {
            if ($field->IsAutoIncrement) {
                continue;
            }
            $stmt->bindParam(':' . $field->FieldName, $entity->{$field->FieldName}, $field->FieldType);
        }
        // Execute
        if (!$stmt->execute()) {
            $arr = $stmt->errorInfo();
            die('SQL-Statement Error: ' . $arr[2]);
        }
        $entity->IsNew = false;
        // Assign AutoIncrement if needed
        if (isset($autoIncField)) {
            $lastId = $pdo->lastInsertId();
            $entity->{$autoIncField->FieldName} = $lastId;
        }
        // Cleanup
        $stmt = null;
        $pdo = null;
    }

    private function Update(SDAEntityBase $entity, array $excludeFields = null) {
        $mapping = self::GetMappingFromEntity($entity);
        $primaryField = null;
        // Build the Update Command
        $cmd = 'UPDATE ' . SDAUtils::Escape($mapping->TableName) . ' SET ';
        foreach ($mapping->FieldList as $field) {
            if ($field->IsPrimary) {
                $primaryField = $field;
            }
            if ($field->IsAutoIncrement) {
                continue;
            }
            if ($excludeFields != null && count($excludeFields) > 0) {
                if (in_array($field->FieldNumber, $excludeFields)) {
                    // Skip excluded Fields
                    continue;
                }
            }
            $cmd .= SDAUtils::Escape($field->FieldName) . ' = ' . ':' . $field->FieldName . ', ';
        }
        $cmd = substr($cmd, 0, -2);
        // Add the Primary Where
        $primaryWhereParam = ':' . $primaryField->FieldName . "_primWhere";
        $cmd .= ' WHERE ' . SDAUtils::Escape($primaryField->FieldName) . ' = ' . $primaryWhereParam;
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        foreach ($mapping->FieldList as $field) {
            if ($field->IsAutoIncrement) {
                continue;
            }
            if ($excludeFields != null && count($excludeFields) > 0) {
                if (in_array($field->FieldNumber, $excludeFields)) {
                    // Skip excluded Fields
                    continue;
                }
            }
            $stmt->bindParam(':' . $field->FieldName, $entity->{$field->FieldName}, $field->FieldType);
        }
        // Bind the Primary Where Param
        $stmt->bindParam($primaryWhereParam, $entity->{$primaryField->FieldName}, $primaryField->FieldType);
        // Execute
        if (!$stmt->execute()) {
            $arr = $stmt->errorInfo();
            die('SQL-Statement Error: ' . $arr[2]);
        }
        // Cleanup
        $stmt = null;
        $pdo = null;
    }

    public function Delete(SDAEntityBase $entity) {
        $mapping = self::GetMappingFromEntity($entity);
        $primaryField = null;
        // Build the Delete Command
        $cmd = 'DELETE FROM ' . SDAUtils::Escape($mapping->TableName);
        // Search the Primary Field
        foreach ($mapping->FieldList as $field) {
            if ($field->IsPrimary) {
                $primaryField = $field;
                break;
            }
        }
        // Add the Primary Where
        $primaryWhereParam = ':' . $primaryField->FieldName . "_primWhere";
        $cmd .= ' WHERE ' . SDAUtils::Escape($primaryField->FieldName) . ' = ' . $primaryWhereParam;
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Primary Where Param
        $stmt->bindParam($primaryWhereParam, $entity->{$primaryField->FieldName}, $primaryField->FieldType);
        // Execute
        if (!$stmt->execute()) {
            $arr = $stmt->errorInfo();
            die('SQL-Statement Error: ' . $arr[2]);
        }
        // Cleanup
        $stmt = null;
        $pdo = null;
    }

    public function GetEntity($entityClass, SDAQuery $query = null) {
        if ($query == null) {
            $query = new SDAQuery();
        }
        $query->Limit = 1;
        $entityList = self::GetEntityList($entityClass, $query);
        if ($entityList != null && count($entityList) > 0) {
            return $entityList[0];
        }
        return null;
    }

    public function GetEntityList($entityClass, SDAQuery $query = null) {
        $resultArray = array();
        $mapping = self::GetMappingFromClass($entityClass);
        // Build the Select Command
        $cmd = 'SELECT ';
        foreach ($mapping->FieldList as $field) {
            if ($query != null && count($query->ExcludeFields) > 0) {
                if (in_array($field->FieldNumber, $query->ExcludeFields)) {
                    // Skip excluded Fields
                    continue;
                }
            }
            $cmd .= SDAUtils::Escape($field->FieldName) . ', ';
        }
        $cmd = substr($cmd, 0, -2);
        $cmd .= ' FROM ' . SDAUtils::Escape($mapping->TableName);
        // Add additional Stuff from the Query
        $paramList = null;
        if ($query != null) {
            // Add the Where
            $paramList = array();
            $cmd .= self::CreateWhere($mapping, $query, $paramList);
            // Add the Ordering
            $cmd .= self::CreateOrderBy($mapping, $query);
            // Add the Limit
            $cmd .= self::CreateLimitFromQuery($query);
        }
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        if ($paramList != null) {
            foreach ($paramList as $param) {
                $stmt->bindParam($param->ParamName, $param->Value, $param->ParamType);
            }
        }
        // Execute
        if ($stmt->execute()) {
            while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
                // Create the Entity
                $entity = new $entityClass;
                self::FillEntityFromRow($mapping, $entity, $row);
                $entity->IsNew = false;
                // Add the Entity to the Array
                $resultArray[] = $entity;
            }
        } else {
            $arr = $stmt->errorInfo();
            die('SQL-Statement Error: ' . $arr[2]);
        }
        $stmt = null;
        $pdo = null;
        return $resultArray;
    }

    public function GetEntityCount($entityClass, SDAQuery $query = null) {
        $mapping = self::GetMappingFromClass($entityClass);
        // Build the Count Command
        $cmd = 'SELECT COUNT(*)';
        $cmd .= ' FROM ' . SDAUtils::Escape($mapping->TableName);
        // Add additional Stuff from the Query
        $paramList = null;
        if ($query != null) {
            // Add the Where
            $paramList = array();
            $cmd .= self::CreateWhere($mapping, $query, $paramList);
        }
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        if ($paramList != null) {
            foreach ($paramList as $param) {
                $stmt->bindParam($param->ParamName, $param->Value, $param->ParamType);
            }
        }
        // Execute
        if (!$stmt->execute()) {
            $arr = $stmt->errorInfo();
            die('SQL-Statement Error: ' . $arr[2]);
        }
        $count = $stmt->fetchColumn();

        // Cleanup
        $stmt = null;
        $pdo = null;

        return $count;
    }

    public function BatchUpdate($entityClass, array $updateFieldList, SDAQuery $query = null) {
        $mapping = self::GetMappingFromClass($entityClass);
        $batchSize = ($query == null) ? 0 : $query->Limit;
        // Build the Update Command
        $cmd = sprintf('UPDATE %s SET ', SDAUtils::Escape($mapping->TableName));
        // Create an empty Param List
        $paramList = array();
        // Add the Values with need updating
        foreach ($updateFieldList as $updateField) {
            // Get the Field
            $field = $mapping->GetField($updateField->FieldNumber);
            // Create and add the Parameter
            $paramName = ':' . $field->FieldName . '_' . count($paramList);
            $paramList[] = new SDAFieldParameter($paramName, $field->FieldType, $updateField->NewValue);
            // Add the Parameter to the Command
            $cmd .= SDAUtils::Escape($field->FieldName) . ' = ' . $paramName . ', ';
        }
        $cmd = substr($cmd, 0, -2);
        // Add the Where
        if ($query != null) {
            $cmd .= self::CreateWhere($mapping, $query, $paramList);
        }
        // Add the Limit
        $cmd .= self::CreateLimit($batchSize);
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        if ($paramList != null) {
            foreach ($paramList as $param) {
                $stmt->bindParam($param->ParamName, $param->Value, $param->ParamType);
            }
        }
        // Execute
        $totalRowsAffected = 0;
        while (true) {
            if (!$stmt->execute()) {
                $arr = $stmt->errorInfo();
                die('SQL-Statement Error: ' . $arr[2]);
            }
            $rowsAffected = $stmt->rowCount();
            $totalRowsAffected += $rowsAffected;
            if ($rowsAffected == 0 || $batchSize == 0) {
                // Break when finished or no Batchsize was given
                break;
            }
        }
        // Cleanup
        $stmt = null;
        $pdo = null;
        return $totalRowsAffected;
    }

    public function BatchDelete($entityClass, SDAQuery $query = null) {
        $mapping = self::GetMappingFromClass($entityClass);
        $batchSize = ($query == null) ? 0 : $query->Limit;
        // Build the Delete Command
        $cmd = sprintf('DELETE FROM %s ', SDAUtils::Escape($mapping->TableName));
        $paramList = null;
        if ($query != null) {
            // Add the Where
            $paramList = array();
            $cmd .= self::CreateWhere($mapping, $query, $paramList);
            // Add the Limit
            $cmd .= self::CreateLimitFromQuery($query);
        }
        if ($this->DEBUG) {
            echo $cmd;
        }
        // Prepare the Command
        $pdo = $this->_connectionProvider->GetDBConnection();
        $stmt = $pdo->prepare($cmd);
        // Bind the Params
        if ($paramList != null) {
            foreach ($paramList as $param) {
                $stmt->bindParam($param->ParamName, $param->Value, $param->ParamType);
            }
        }
        // Execute
        $totalRowsAffected = 0;
        while (true) {
            if (!$stmt->execute()) {
                $arr = $stmt->errorInfo();
                die('SQL-Statement Error: ' . $arr[2]);
            }
            $rowsAffected = $stmt->rowCount();
            $totalRowsAffected += $rowsAffected;
            if ($rowsAffected == 0 || $batchSize == 0) {
                // Break when finished or no Batchsize was given
                break;
            }
        }
        // Cleanup
        $stmt = null;
        $pdo = null;
        return $totalRowsAffected;
    }

    // Creates the Order By Part of a Query
    private static function CreateOrderBy(SDAMappingInfo $mappingInfo, SDAQuery $query) {
        if ($query != null) {
            if (count($query->OrderFields) > 0) {
                $str = ' ORDER BY ';
                $firstFlag = true;
                foreach ($query->OrderFields as $orderItem) {
                    if (!$firstFlag) {
                        $str .= ', ';
                    }
                    $str .= $orderItem->Generate($mappingInfo);
                    $firstFlag = false;
                }
                return $str;
            }
        }
        return '';
    }

    private static function CreateLimitFromQuery(SDAQuery $query) {
        if ($query != null) {
            return self::CreateLimit($query->Limit);
        }
        return '';
    }

    private static function CreateLimit($limitNumber) {
        if ($limitNumber > 0) {
            return ' LIMIT ' . $limitNumber;
        }
        return '';
    }

    private static function CreateWhere(SDAMappingInfo $mappingInfo, SDAQuery $query, array &$paramList) {
        if ($query != null) {
            if (count($query->ExpressionList) > 0) {
                return ' WHERE ' . $query->GenerateAndFill($mappingInfo, $paramList);
            }
        }
        return '';
    }

    private static function FillEntityFromRow(SDAMappingInfo $mapping, SDAEntityBase $entity, $row) {
        foreach ($row as $key => $value) {
            $entity->{$key} = $value;
        }
    }

    private static function GetMappingFromEntity(SDAEntityBase $entity) {
        $entityClass = get_class($entity);
        return self::GetMappingFromClass($entityClass);
    }

    private static function GetMappingFromClass($entityClass) {
        $vars = get_class_vars($entityClass);
        return $vars['MappingInfo'];
    }

}
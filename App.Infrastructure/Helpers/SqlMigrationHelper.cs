﻿using System.Collections.Generic;

namespace App.Infrastructure.Helpers
{
    public class SqlMigrationHelper
    {
        public static List<ColumnStruct> GetBaseColumns() => new List<ColumnStruct> {
                  new ColumnStruct{ colName = "LastTransactionAction",dataType = "nvarchar(2)" },
                  new ColumnStruct{ colName = "AddTransactionUser",dataType = "nvarchar(20)" },
                  new ColumnStruct{ colName = "AddTransactionDate",dataType = "nvarchar(10)" },
                  new ColumnStruct{ colName = "LastTransactionUser",dataType = "nvarchar(20)" },
                  new ColumnStruct{ colName = "LastTransactionDate",dataType = "nvarchar(10)" } ,
                  new ColumnStruct{ colName = "DeleteTransactionUser",dataType = "nvarchar(20)" } ,
                  new ColumnStruct{ colName = "DeleteTransactionDate",dataType = "nvarchar(10)" }
        };

        /// <summary>
        /// Add Schema If Not Exists
        /// </summary>
        /// <param name="SchemaName"></param>
        /// <returns></returns>
        public static string AddSchemaIfNotExists(string SchemaName) => string.Format(@"IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = '{0}' )
    EXEC('CREATE SCHEMA [{0}]')", SchemaName);

        /// <summary> AddColumnIfNotExists
        /// Add Column If Not Exists
        /// </summary>
        /// <param name="tableName"> Specified Table For The Column To be added </param>
        /// <param name="columnNameWithDT"> Column Name With Its Data Type And Constraints </param>
        /// <returns></returns>
        public static string AddColumnIfNotExists(string tableName, string columnName,string dataType,string schemaName = "") => string.Format(@"IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'{3}{0}') 
         AND name = '{1}'
) EXEC('ALTER TABLE {0} ADD {1} {2}')", tableName, columnName, dataType, string.IsNullOrEmpty(schemaName) ? "." + schemaName : "");
        /// <summary> AddSequenceIfNotExists
        /// Check For Sequence If Created Or Not
        /// If Not Then Create New Sequence
        /// </summary>
        /// <param name="sequenceName"> Sequence Should Be Name Of Table Plus _SEQ </param>
        /// <param name="startWith"> The First Number To Be Iterate After </param>
        /// <param name="minValue"> Minimum Value </param>
        /// <returns></returns>
        public static string AddSequenceIfNotExists(string sequenceName, int startWith, int minValue) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM all_sequences where sequence_name = '{0}';
               IF(nCount <= 0)
               THEN
               v_sql:='CREATE SEQUENCE {0}
               START WITH {1}
               MAXVALUE 999999999999999999999999999
               MINVALUE {2}
               NOCYCLE
               CACHE 20
               NOORDER';             
               execute immediate v_sql;
               END IF;
               end;", sequenceName.ToUpper(), startWith, minValue);

        /// <summary> AddTriggerForSequence
        /// Create Trigger To Fire Sequence Each Time There is an insert on database
        /// </summary>
        /// <param name="tableName"> Table To Be Triggered </param>
        /// <param name="sequenceName"> Sequence Thats Created For Table  </param>
        /// <param name="columnToBeSequenced"> Auto Incremental Column ( Should Be The Primary Key
        /// ) </param>
        /// <returns></returns>
        public static string AddTriggerForSequence(string tableName, string sequenceName, string columnToBeSequenced) => string.Format(@"CREATE OR REPLACE TRIGGER {0} 
BEFORE INSERT ON {0} 
FOR EACH ROW
BEGIN
  SELECT {1}.NEXTVAL
  INTO   :new.{2} 
  FROM   dual;
END;", tableName, sequenceName, columnToBeSequenced);

        public static string AddTriggerForSequence(string tableName, string sequenceName, string columnToBeSequenced, string triggerName) => string.Format(@"CREATE OR REPLACE TRIGGER {3} 
BEFORE INSERT ON {0} 
FOR EACH ROW
BEGIN
  SELECT {1}.NEXTVAL
  INTO   :new.{2} 
  FROM   dual;
END;", tableName, sequenceName, columnToBeSequenced, triggerName);

        /// <summary> AddRowIfNotExists
        /// Check if row is exists then insert it
        /// </summary>
        /// <param name="tableName">example : Table1</param>
        /// <param name="columns">example : col1,col2,col3</param>
        /// <param name="values">example : 'val1',val2</param>
        /// <param name="condition">example : id=1 and name = 'ahmed'</param>
        /// <returns></returns>
        public static string AddRowIfNotExists(string tableName, string columns, string values, string condition) => string.Format(@"Insert into {0}
   ({1})
SELECT {2}
FROM dual
WHERE NOT EXISTS (SELECT *
                  FROM {0}
                  WHERE {3});", tableName, columns, values, condition);

        public static string DropConstraintIfExists(string constraintName, string tableName) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM USER_CONSTRAINTS  where CONSTRAINT_NAME  = '{0}';
               IF(nCount > 0)
               THEN
               v_sql:='ALTER TABLE {1} DROP CONSTRAINT {0}';             
               execute immediate v_sql;
               END IF;
               end;", constraintName, tableName);
        public static string AddIndexIfNotExists(string indexName, string tableName, string[] columns) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM USER_INDEXES  where INDEX_NAME  = '{0}';
               IF(nCount <= 0)
               THEN
               v_sql:='CREATE UNIQUE INDEX {0} ON {1} ({2})';             
               execute immediate v_sql;
               END IF;
               end;", indexName, tableName, string.Join(",", columns));
        public static string AddPrimaryKeyIfNotExists(string indexName, string tableName, string[] baseColumns, bool enableVlaidate) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM USER_CONSTRAINTS  where CONSTRAINT_NAME  = '{0}';
               IF(nCount <= 0)
               THEN
               v_sql:='ALTER TABLE {1} ADD (CONSTRAINT {0} PRIMARY KEY ({2}) USING INDEX {0} {3})';             
               execute immediate v_sql;
               END IF;
               end;", indexName, tableName, string.Join(",", baseColumns), enableVlaidate ? "ENABLE VALIDATE" : "");
        public static string AddForeignKeyIfNotExists(string constraintName, string tableName, string[] baseColumns, string referenceTable, string[] referenceColumns, bool deleteCascade) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM USER_CONSTRAINTS  where CONSTRAINT_NAME  = '{0}';
               IF(nCount <= 0)
               THEN
               v_sql:='ALTER TABLE {1} ADD (CONSTRAINT {0} FOREIGN KEY ({2}) REFERENCES {3} ({4}) {5} ENABLE VALIDATE)';             
               execute immediate v_sql;
               END IF;
               end;", constraintName, tableName, string.Join(",", baseColumns), referenceTable, string.Join(",", referenceColumns), deleteCascade ? "ON DELETE CASCADE" : "");
        public static string DropIndexIfExists(string indexName) => string.Format(@"declare
               nCount NUMBER;
               v_sql LONG;
               begin
               SELECT count(*) into nCount FROM USER_INDEXES  where INDEX_NAME  = '{0}';
               IF(nCount > 0)
               THEN
               v_sql:='DROP INDEX {0}';             
               execute immediate v_sql;
               END IF;
               end;", indexName);
        public static string DeleteColumnIfExists(string tableName, string columnName) => string.Format(@"DECLARE
  l_cnt integer;
BEGIN
  SELECT COUNT(*)  into l_cnt
    FROM dba_tab_columns
   WHERE 
     table_name = '{0}'
     AND column_name = '{1}';

  IF( l_cnt = 1 )
  THEN
    EXECUTE IMMEDIATE 'ALTER TABLE {0} DROP COLUMN {1}';
  END IF;
END;", tableName, columnName);
        public static string ModifyColumnIfExists(string tableName, string columnName, string DataType) => string.Format(@"DECLARE
  l_cnt integer;
BEGIN
  SELECT COUNT(*)  into l_cnt
    FROM dba_tab_columns
   WHERE 
     table_name = '{0}'
     AND column_name = '{1}';

  IF( l_cnt = 1 )
  THEN
    EXECUTE IMMEDIATE 'ALTER TABLE {0} MODIFY {1} {2}';
  END IF;
END;", tableName, columnName, DataType);
        public static string RenameColumnIfExists(string tableName, string columnName, string NewColumnName) => string.Format(@"DECLARE
  l_cnt integer;
BEGIN
  SELECT COUNT(*)  into l_cnt
    FROM dba_tab_columns
   WHERE 
     table_name = '{0}'
     AND column_name = '{1}';

  IF( l_cnt = 1 )
  THEN
    EXECUTE IMMEDIATE 'ALTER TABLE {0} RENAME COLUMN  {1} TO {2}';
  END IF;
END;", tableName, columnName, NewColumnName);

        public static string AddTableIfNotExists(string tableName, string Schema = "") => string.Format(@"IF NOT EXISTS 
(SELECT * FROM sys.tables WHERE name='{0}')
    EXEC('CREATE TABLE {1}{0} (
        BranchId int Not null
    )')", tableName, string.IsNullOrEmpty(Schema) ? "." + Schema : "");

        public static string AddTableIfNotExists(string tableName, string columnName, string dataType, string constraint = "",string Schema = "") => string.Format(@"IF NOT EXISTS 
(SELECT * FROM sys.tables WHERE name='{0}')
  EXEC('CREATE TABLE {4}{0} (
        {1} {2} {3}
    )' ) ", tableName, columnName, dataType, constraint,string.IsNullOrEmpty(Schema) ? "." + Schema : "");
    }
    public class ColumnStruct
    {
        public string colName { get; set; }
        public string dataType { get; set; }
        public string constraint { get; set; }
    }
}



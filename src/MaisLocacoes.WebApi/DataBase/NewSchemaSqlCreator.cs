using System.Text;

namespace MaisLocacoes.WebApi.Context
{
    public class NewSchemaSqlCreator
    {
        public static string SqlQueryForNewSchema(string newSchemaName)
        {
            var stringBuilder = new StringBuilder();
            //Cria um schema cópia do schema 'public' para uma nova company cadastrada
            stringBuilder.Append(@$"CREATE SCHEMA IF NOT EXISTS ""{newSchemaName}"";
                                    DO $$DECLARE r record;
                                    BEGIN
                                    FOR r IN(SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
                                    EXECUTE 'CREATE TABLE IF NOT EXISTS ""{newSchemaName}"".' || quote_ident(r.tablename) || ' (LIKE public.' || quote_ident(r.tablename) || ' INCLUDING ALL)';
                                    END LOOP;
                                    END$$;");

            return stringBuilder.ToString();
        }

        public static string SqlQueryForAtualizeSchemasTablesAndFks(string newSchemaName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                @$"DO $$
				DECLARE
				  atualschemaname_schema_name text;
				  r record;
				  fk_name text;
				  result_text text := '';
				  result_text2 text := ''; 
				  schema_table text := '';
				  schema_table2 text := '';
				  referenced_table text := '';
				  item record;
				  item2 record;
				  curr_col text := '';
				  fk_exists boolean := false;
				BEGIN
				  
				  atualschemaname_schema_name := '{newSchemaName}';

				  FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public' AND tablename != '__EFMigrationsHistory')
					LOOP
						EXECUTE 'CREATE TABLE IF NOT EXISTS ' || quote_ident(atualschemaname_schema_name)  || '.' || quote_ident(r.tablename) || ' (LIKE public.' || quote_ident(r.tablename) || ' INCLUDING ALL)'; 
      					IF Exists (SELECT conname FROM pg_constraint c WHERE conrelid::regclass = concat('public', '.', '""', r.tablename, '""')::regclass AND contype IN ('f')) 
						THEN
						FOR fk_name IN (SELECT conname FROM pg_constraint c WHERE conrelid::regclass = concat('public', '.', '""', r.tablename, '""')::regclass AND contype IN ('f')) 
						LOOP
													SELECT EXISTS (SELECT 1 FROM information_schema.table_constraints
													WHERE constraint_type = 'FOREIGN KEY'
													AND table_schema = atualschemaname_schema_name
													AND table_name = r.tablename
													AND constraint_name = fk_name
													) INTO fk_exists;
                                    						IF NOT fk_exists THEN
															curr_col := '';
															result_text := '';
															schema_table = 'public.' || '""' || r.tablename || '""';
															FOR item IN (SELECT attname 
															FROM pg_attribute 
															WHERE attrelid = schema_table::regclass 
															AND attnum IN (SELECT unnest(conkey) AS i
															FROM pg_constraint 
															WHERE conname = fk_name 
															AND conrelid = schema_table::regclass 
															AND contype = 'f')) ORDER BY attnum
													LOOP
														    IF curr_col <> '' THEN
															result_text := concat(result_text, ', ');
															END IF;
															curr_col := item.attname;
															result_text := concat(result_text, quote_ident(curr_col));
												    END LOOP;
															curr_col := '';
															result_text2 := '';
															referenced_table = split_part(fk_name, '_', 3);
															schema_table = 'public.' || '""' || referenced_table || '""';
															FOR item2 IN(SELECT 
														    attname AS column_name
															FROM 
															pg_index, pg_class, pg_attribute
															WHERE 
															pg_class.oid = schema_table::regclass AND
															indrelid = pg_class.oid AND
															pg_attribute.attrelid		 = pg_class.oid AND
															pg_attribute.attnum = any(pg_index.indkey)
															AND indisprimary) ORDER BY attnum DESC
													LOOP
															IF curr_col <> '' THEN
															result_text2 := concat(result_text2, ', ');
															END IF;
															curr_col := item2.column_name;
															result_text2 := concat(result_text2, quote_ident(curr_col));
												   END LOOP;
															EXECUTE format('ALTER TABLE IF EXISTS %I.%I ADD CONSTRAINT %I FOREIGN KEY (%s) 
															REFERENCES %I.%I (%s) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE CASCADE',
															atualschemaname_schema_name, r.tablename, fk_name, result_text, atualschemaname_schema_name, referenced_table, result_text2);
                                    						END IF;
				
				END LOOP;
				END IF;
				END LOOP;
				END $$;");

            return stringBuilder.ToString();
        }
    }
}
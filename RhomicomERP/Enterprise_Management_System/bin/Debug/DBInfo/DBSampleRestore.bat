cd /D C:\Program Files (x86)\pgAdmin 4\v2\runtime

pg_restore.exe --host 192.168.56.200  --port 5432 --username postgres --clean --schema-only --dbname "all_db_schemas" --verbose "C:\Users\Richard\Google Drive\PROJECTS\NON-GITHUB\RHO_ERP_DESKTOP\RhomicomERP\Enterprise_Management_System\bin\Debug\prereq\test_database.backup"

pg_restore.exe --host 192.168.56.200  --port 5432 --username postgres --data-only --dbname "all_db_schemas" --verbose "C:\RhomicomERP_V1\prereq\test_database.backup"

xcopy "C:\Users\Richard\Google Drive\PROJECTS\NON-GITHUB\RHO_ERP_DESKTOP\RhomicomERP\Enterprise_Management_System\bin\Debug\prereq\Images\*.*" "C:\RhomicomERP_V1\Images\all_db_schemas\" /E /I /-Y /F /C

PAUSE

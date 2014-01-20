Insightly Database Sync
=======================

Syncs remote data from an Insightly CRM account with a local SQL Server database.

This is a "hard" sync.  It's one way, and it actually drops all the tables in the database before writing new data.  So this only allows a local copy of your Insightly data for querying and reporting.  It does not push anything into Insightly, and you shouldn't extend the database tables with your own data, because they get wiped out prior to rewriting.

To Run
------

1. Compile to an EXE.

2. Put these files in the same folder

 * InsightyApi.exe
 * Log4Net.dll
 * InsightlyApi.exe.config
 * Log4Net.dll
 * Log4Net.config
 * reset.sql
 * schema.sql

3. In InsightlyApi.exe.config change two settings:

 * Input a connection string to your SQL Server.  The user should have DBO access to an empty database.
 * Input your Insightly API key.

4. Run from the command line.

At the default, it will download and sync Contacts, Organizations, and Projects.

To Extend
---------

To add new objects, you will need to write code -- though it's very simple code.  It's essentially configuration-by-code.

1. Design your database table.  Add whatever columns you like -- you will map them to data in the class defintion below. (See "Data Mapping" below.)

2. Add your new table to the schema.sql file.  This file is executed on every run, after reset.sql blows everything away.

3. Add the new object type that you want to collect "ToDownload" app setting in the config file.  There's a comment in the config which shows all the options.

4. Create a new class under the InsightlyApi.InsightlyObjects namespace. Extend from InsightlyObject.  The files are in the InsightlyObjects folder.

5. Add this attribute to your new class defintion.

    [InsightlyTableMapping(ObjectName = "Contact", TableName = "Contacts")]

 ObjectName should be one of the values in the "ToDownload" string in the config file.  TableName should be the name of the corresponding table in SQL Server.

6. Add properties with this attribute.

    [InsightlyColumnMapping]

   This maps the property to a database column _of the same name_.  If the column has a different name than the property, you can specify this:

    [InsightlyColumnMapping(ColumnName = "SomeOtherName")]

7. Inside your property implementation, query the XML and return the string to write to that database column.  There are helper methods inside the InsightlyObject base class.

8. Recompile and replace the EXE.

Data Mapping
--------------------
Each time the program executes, it saves the downloaded XML inside a directory called "data".  So, at any time, you should have the downloaded XML for the last run.  You will need to refer to this when writing your property implementations so you know how to extract the different XML values into your properties.

_The properties of your classes (and subsequent database columns) don't have to mirror the XML elements_.  You can return whatever you want from the properties and it will write to the database column.  So you can combine XML elements into one field, check values and return constants, even do external lookups and return that.  The system will write whatever that property returns into your database field, so go nuts.  (For an example, see the "Name" property in the "InsightlyContact" class.)

Warning
---------
This code is rough. It has been used internally only, and as of January 2014 has never been reviewed or used outside my organization.  It has been very stable for us, but I provide absolutely no warranty, and you should probably expect the worst.

-- Deane, deane@blendinteractive.com
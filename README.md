I hope you will like it :) 


The list of my suggestions to the initial code can be found here and in a seperate file called "suggestions.txt"


Below is a list of things that immediately caught my attention: 

I will group it for each class:


 DataReader.cs
  1. Placing multiple classes in the same file is probably not a good idea, 
     we can extract ImportedObject and ImportedObjectBaseClass into seperate files.

  2. Line 10: the field is missing a scope modifier: we should probably specify the scope and use
     appropriate case (importedObjects or _importedObjects if private, ImportedObjects if public).
     However, the method name is "ImportAndPrintData" and if we make this field private, there is actually
     no way of accessing it after importing. We can either change the method return type or declare this field
     as a property with a public getter. I will mark it as a property with a public getter (returning a copy).
 
  3. Line 14: we have a parameter "printData" but we never use it, and the method name implies that we will print the data
     eventually, so this parameter is redundant.

  4. Line 16: We initialize a List of "ImportedObject" with one empty object. This will throw a null reference on 
     line 45, when we will try to "clear and correct" this data. 

  5. The method "ImportAndPrintData" looks a bit long, we will split it into smaller, private methods to make it
     easier to refactor in the future. 

  6. Line 18 The class "StreamReader" indirectly implements IDisposable, it would be a good idea to use "using" clause in order
     to close the resources properly to avoid any tiresome and hard to fix errors.
     Also, program will fail if line is empty (we need to guard against it)
     IMPORTANT: we will move the code using resources to a seperate method to make disposing easier.

  8. Line 27: We can extract this as sort of a "mapper", accepting a line and returning the object with values fetched from the line.

  9. Line 39: These casts don't look to well, we can use IEnumerable<ImportedObjects>.ToList()

  10. Line 42: We should extract this "correct" method.

  10. Line 50: We should extract "assign number of children" to a seperate method.

  11. Line 68: We should extract this long block into a method and give a meaningful name (maybe "PrintDatabaseDetails" ?)

  12. Line 98: Not sure if Console.ReadLine() is necessary here.

      GENERAL: We know that the data provided is in CSV. Why not use a library? We will utilize CsvHelper in the solution.
	(Using of direct access of the array[i] on lines 29 through 38 is a bit worrying and would easily get too complicated)
	(Also, any changes in the csv structure would mean a total collapse of this solution)
	(CsvHelper will map headers to props so we won't have to worry about the ordering or going out of bounds and we will be notified if something goes wrong quickly :) )
	  Since we know that this method reads from CSV only, let's make that clear by changing the name method.
	Additionally, if in future we need some other sources of data (not necessarily CSV), let's create an interface called "IDataReader" and 
	put our future contracts there. 
	Should anyone ever need to use this in an IOC way, we can just pass the interfaces instead of the concrete implementation classes.
	!!! Also, it would be great to give the user a list of rows which could not be parsed, for clarity and easy problem detection on the data side. !!! 
      OTHER GENERAL NOTES: 
	We could utilize LINQ heavily here, it would make each line more readable and compact.
	We will re-organize any newly created files into folders to make it easier in case of the program's extension in the future. 
	(it's hard to see on small-scale apps, but a properly designed folder structure makes life much easier :) )

  ImportedObject class:

  1. We already specified DataType {get; set;}, we should probably be consistent and
     replace Name property with Name {get; set;} aswell ParentType: same thing. (more of an eye-candy)
  
  2. NumberOfChildren should probably have a pulic getter.
	We probably won't work with doubles as NumbersOfChildren is inherently an integer, why not change it to int?

  3. "Name" property is unnecessary, we have it in our super class we inherit from.

  ImportedObjectBaseClass: 

  1. We could avoid the "Class" suffix, I think "ImportedObjectBase" would look fine (interesting thing to note: naming things inherently ties them to 
     their initial role and makes it really hard to change in the future - what if "ImportedObjectBaseClass" would have to become abstract? would we have
     to rename all it's occurences across the code as "ImportedObjectBaseAbstractClass" ?).

ALSO IN REGARDS TO THE WHOLE PROJECT: 
We are missing docs! let's make it more clear what our methods and classes do in the corrected version.



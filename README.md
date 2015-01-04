ExpenseCategorizer
==================

Tool for categorizing and summarizing bank statements from CSV data.

##Usage
* 1. Create a category mapping (see section below).
* 2. Fetch the transaction data from your bank in a CSV format (instructions for Länsförsäkringar in ExpenseCategorizer.Utils.InputPreparation)
* 3. Run application: expensecategorizer `[path to transaction csv] [path to category file]`
* 4. View result.csv and unknown.txt
* 5. Done! :) 

##Transaction data

Transaction data file can be any tab separated CSV file. Columns may come in any order.

Example:
```
2015-01-02	2015-01-02	Store X	-100	1000
2015-01-01	2014-12-31	Store Y	-50,5	1100	
2015-01-01	2014-12-30	Store X	-25,00	1150,5
```

##Category mapping

This is the part that sets up the caterization of each transaction. This is done in a simple XML format (schema in ExpenseCategorizer.Utils.Schema).
Each input tag supports regular expressions.

Example:
```
<categories>
  <category name="Food">
    <input>Store X</input>
  </category>
  <category name="Clothing">
    <input> Y$</input>
  </category>
</categories>

```

##Result
The result will be written to the console and saved as result.csv in the same folder as the executable.

Example:
```

Category	Total Average 2015-01
Clothing	50,5	50,50	50,5
Food	125,00	125,00	125,00

```

If unknown transactions are found (i.e transactions where no category matches) there will also be a file called unknown.txt 
containing the name of these transactions.

##License
[MIT](https://github.com/smatsson/ExpenseCategorizer/blob/master/LICENSE)

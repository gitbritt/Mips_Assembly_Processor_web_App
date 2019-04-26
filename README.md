# Mips Assembly Web APP

This is an ASP.NET web app that checks students homework for a single cycled MIPS processor.



![alt text](https://github.com/gitbritt/222_web_app/blob/master/Single_cycled_processor.jpg)


# Tips for setting up
The list of instructions is in a CSV file under Register_file/instructions.csv. This is where you can modify the typs of instructions being generated. 


To specify how many rows you want the program to read in the CSV file, find int operation_row_file = operation.Next(2, 27);
and change it to int operation_row_file = operation.Next(2, #of rows to read);

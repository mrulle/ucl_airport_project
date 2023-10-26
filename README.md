# MONKEY
<h1 align="center">
	<img alt="cgapp logo" src="https://github.com/mrulle/ucl_airport_project/blob/master/monkey.jpeg" width="224px"/><br/>
	Monkeys
</h1>
En gruppe af abekatte der ihærdigt prøver at komme igennem 2. semester softwareudvikling, og hvis det går galt, så er der jo altid web udvikling.

# Ting der mangler
* Et view hvor man kan se om en person allerede er checket in, da der på database niveau ikke kan tjekke ind 2 gange på samme tid, men at frontenden ikke får nogen respons.

# Warning for database
* Hvis det ikke virker så husk at sætte vscode til LF i stedet for CRLF, for ellers kan databasefilerne ikke køres ordentligt.
* Hvis det heller ikke virker så prøv at se om der mangler permissions, i tilfælde af at det køres på en Linux maskine (bare chmod 777, setup.sh og de to db filer).

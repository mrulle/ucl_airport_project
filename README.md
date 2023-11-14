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

# RabbitMQ logging

## Message logging

Lav en fil ved navn `enabled_plugins` med indholdet `[rabbitmq_management,rabbitmq_prometheus,rabbitmq_tracing].`. Filen bliver læst som en Erlang fil.

Få den mounted ind i RabbitMQ docker containeren, eksempelvis som en option i en `docker run` : `-v ./enabled_plugins:/etc/rabbitmq/enabled_plugins`.

Aktiver tracing i RabbitMQ. Typisk gøres det fra CLI'en i rabbitmq container instansen: `rabbitmqctl trace_on`. Der kan findes lidt mere information om options til den kommando: https://www.rabbitmq.com/firehose.html

Derefter skal der laves et `trace` ved admin interfacet på rabbitmq. Det bør undersøges hvordan dette kan laves automatisk. Brug trace pattern `#` til at trace alt.
Alt efter hvad trace navnet blev givet. Kan traces ses i filen ved placeringen på rabbitmq docker instansen: `/var/tmp/rabbitmq-tracing/<trace_log_navn>.log`

## Queue & Exchange logging

Man kan se *internal events* ved at eksekvere `rabbitmq-diagnostics consume_event_stream` på rabbitmq docker instansen.
Rabbitmq's dokumentation siger at disse events kan *consumes* med et plugin så de kan sendes til applikationer (e.g. Logstash): https://www.rabbitmq.com/event-exchange.html

## Logging af Connections & Channels

source: https://www.rabbitmq.com/logging.html
RabbitMQ skal sættes til at logge til `file` i stedet for `console`. Og `log level` skal sættes til debug.
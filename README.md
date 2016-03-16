# HomeIot

This project is a reboot of the home automation project that is available in DomoSI repository.

## Target

This project target mainly to aspect of the home automation :
* Reading the power consumption indexes
* Controlling home heaters using the wire command used mainly in France

## Hardware

This is the main change from the previous project :

Instead of using the BeagleBone Black, this project will use Raspberry pi under Windows 10 IOT
The main WebServer, Service and database computer is a PC under Windows Server and running Sql Server 2014
The communication layer is using a RabbitMq server with the MQTT protocol activated.

In the long run i will also adds collecting data nodes using Arduino, ESP8266 or stm32.

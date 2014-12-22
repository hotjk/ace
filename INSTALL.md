# Grit.Demo.ACE #

### Setup RabbitMQ ###

* Install RabblitMQ
* Start RabbitMQ service
* Add vhost/user/permission

rabbitmqctl add_vhost grit
rabbitmqctl set_permissions -p grit guest ".*" ".*" ".*"

### Setup MySQL ###

* Install MySQL
* Import Dump20140707.sql

### Run ###

* ACE.Demo.MicroServices
* ACE.Demo.EventConsumer
* ACE.Demo.Heavy.Web

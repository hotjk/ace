# Grit.Demo.CQRS #

### Setup RabbitMQ ###

* Install RabblitMQ
* Start RabbitMQ service
* Add vhost/user/permission

rabbitmqctl add_vhost event_bus_vhost

rabbitmqctl add_user event_user event_password

rabbitmqctl set_permissions -p event_bus_vhost event_user ".*" ".*" ".*"

rabbitmqctl set_permissions -p event_bus_vhost guest ".*" ".*" ".*"

### Setup MySQL ###

* Install MySQL
* Import Dump20140707.sql

### Run ###

* CQRS.Demo.Sagas
* CQRS.Demo.EventConsumer
* CQRS.Demo.Web
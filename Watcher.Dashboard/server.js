var redis = require('redis');
var express = require('express');
var app = express();

app.get('/', function (req, res) {
    res.send('Hello World!');
});

app.get('/event/:event/:period', function (req, res) {
    var client = redis.createClient(6379, 'localhost');
    var event = req.params.event;
    var period = req.params.period;
    client.hgetall(event + '_' + period, function (err, object) {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(object));
    });
});

app.use(express.static('public'));

app.listen(3000, function () {
    console.log('Example app listening on port 3000!');
});

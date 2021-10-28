var express = require('express');
var server = express();
var options = {
  index: 'index.html',
};
server.use('/', express.static('/home/site/wwwroot', options));
server.listen(8080);
server.get('/*', function(req, res) {
  res.sendFile('/home/site/wwwroot/index.html', function(err) {
    if (err) {
      res.status(500).send(err);
    }
  });
});

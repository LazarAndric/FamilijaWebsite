const proxy = require("http-proxy-middleware");
const cors = require("cors");

module.exports = function (app) {
  app.use(
    proxy("", {
      target: "",
      changeOrrigin: true,
    })
  );
  app.use(cors());
};

const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:38593';

const context = [
    "/api/user/GetUserDetails",
    "/api/weatherforecast",
    "/api/products/get",
    "/api/products/GetUserProducts",
    "/api/products/GetProduct",
    "/api/cart",
    "/api/checkout",
    "/api/feedback",
    "/api/user/register",
    "/api/user/login",
    "/api/user/edit",
    "/api/user/GetOrderHistory"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};

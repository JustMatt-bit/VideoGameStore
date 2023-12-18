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
    "/api/products/CreateProduct",
    "/api/products/GetGenres",
    "/api/products/UploadImage",
    "/api/products/GetGameTypes",
    "/api/products/GetDevelopers",
    "/api/products/CreateDeveloper",
    "/api/products/DeleteDeveloper",
    "/api/products/GenreExists",
    "/api/products/CreateGenre",
    "/api/products/GetProductGenres",
    "/api/products/UpdateProduct",
    "/api/products/DeleteGenres",
    "/api/products/GenresProductConnection",
    "/api/products/UpdateGenresProductConnection",
    "/api/cart",
    "/api/checkout",
    "/api/feedback",
    "/api/user/register",
    "/api/user/login",
    "/api/user/edit",
    "/api/user/GetCurrentUser",
    "/api/user/GetOrderHistory",
    "/api/leaderboard/GetTopUsers",
    "/api/leaderboard/GetUserPosition",
    "/api/loyalty/GetUserTierDetails",
    "/api/referral/CheckReferralCode",
    "/api/referral/GenerateReferralCode",
    "/api/discount/applyDiscount",
    "/api/discount/userDiscounts",
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

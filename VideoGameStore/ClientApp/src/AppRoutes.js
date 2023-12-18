import { Home } from "./components/Home";
import { FetchProducts } from "./components/FetchProducts";
import { FetchAccount } from "./components/FetchAccount";
import { FetchLeaderboard } from "./components/FetchLeaderboard";
import { FetchOrderHistory } from "./components/FetchOrderHistory";
import { EditAccount } from "./components/EditAccount";
import { FetchOrder } from "./components/FetchOrder";
import { Register } from "./components/Register";
import { FetchFeedback } from "./components/FetchFeedback";
import { Cart } from "./components/Cart";
import { RecommendationList } from "./components/RecommendationList";
import { Product } from "./components/Product";
import { ProductControl } from "./components/ProductControl";
import { ProductCreate } from "./components/ProductCreate";
import { ProductEdit } from "./components/ProductEdit";
import { GenreCreate } from "./components/GenreCreate";
import { GenresDelete } from "./components/GenresDelete";
import { Checkout } from "./components/Checkout";
import { Shipping } from "./components/Shipping";
import { LoyaltyProgram } from "./components/LoyaltyProgram";
import { Verify } from "./components/Verify";



const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/fetch-products',
    element: <FetchProducts />
  },
  {
    path: '/shipping',
    element: <Shipping />
  },
  {
      path: '/fetch-account',
      element: <FetchAccount />
    },
    {
        path: '/fetch-leaderboard',
        element: <FetchLeaderboard />
    },
    {
        path: '/fetch-order-history',
        element: <FetchOrderHistory />
    },
    {
        path: '/edit-account',
        element: <EditAccount />
    },
    {
        path: '/fetch-order/:id',
        element: <FetchOrder />
    },
    {
        path: '/cart',
        element: <Cart />
    },
    {
        path: '/fetch-feedback',
        element: <FetchFeedback />
    },
    {
        path: '/register',
        element: <Register />
    },
    {
        path: '/checkout',
        element: <Checkout />
    },
    {
        path: '/recommendation-list',
        element: <RecommendationList />
    },
    {
        path: '/product',
        element: <Product />
    },
    {
        path: '/product-control',
        element: <ProductControl />
    },
    {
        path: 'product-create',
        element: <ProductCreate />
    },
    {
        path: 'genre-create',
        element: <GenreCreate />
    },
    {
        path: 'genres-delete',
        element: <GenresDelete />
    },
    {
        path: 'product-edit',
        element: <ProductEdit />
    },
    {
        path: 'fetch-loyalty',
        element: <LoyaltyProgram />
    },
    {
        path: '/verify',
        element: <Verify />
    }
];

export default AppRoutes;

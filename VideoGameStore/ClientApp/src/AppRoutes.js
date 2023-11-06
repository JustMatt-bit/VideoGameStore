import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { FetchProducts } from "./components/FetchProducts";
import { FetchFeedback } from "./components/FetchFeedback";
import { Cart } from "./components/Cart";
import { RecommendationList } from "./components/RecommendationList";
import { Product } from "./components/Product";
import { ProductControl } from "./components/ProductControl";
import { ProductCreate } from "./components/ProductCreate";
import { ProductEdit } from "./components/ProductEdit";
import { Checkout } from "./components/Checkout";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    },
    {
        path: '/fetch-products',
        element: <FetchProducts />
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
        path: 'product-edit',
        element: <ProductEdit />
    }

];

export default AppRoutes;

import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { FetchProducts } from "./components/FetchProducts";
import { RecommendationList } from "./components/RecommendationList";

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
    }
    ,
    {
        path: '/recommendation-list',
        element: <RecommendationList />
    }
];

export default AppRoutes;

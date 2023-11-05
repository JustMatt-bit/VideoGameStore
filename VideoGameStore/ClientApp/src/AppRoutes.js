import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { FetchProducts } from "./components/FetchProducts";
import { FetchFeedback } from "./components/FetchFeedback";
import { Cart } from "./components/Cart";

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
  }
];

export default AppRoutes;

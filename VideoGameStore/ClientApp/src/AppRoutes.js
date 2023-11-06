import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { FetchProducts } from "./components/FetchProducts";
import { FetchAccount } from "./components/FetchAccount";

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
      path: '/fetch-account',
      element: <FetchAccount />
  }
];

export default AppRoutes;

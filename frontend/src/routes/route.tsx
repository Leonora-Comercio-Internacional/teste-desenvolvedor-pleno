import { createBrowserRouter } from "react-router-dom";
import { SignIn } from "../pages/signIn";
import { Product } from "../pages/product";
import { SignUp } from "../pages/signUp";
import { AuthLayout } from "./AuthLayout";
import { ProtectedLayout } from "./ProtectedLayout";

export const router = createBrowserRouter([
  {
    path: '/auth',
    element: <AuthLayout />,
    children: [
      {
        path: 'signIn',
        element: <SignIn />
      },
      {
        path: 'signUp',
        element: <SignUp />
      },
    ]
  },
  {
    path: '/app',
    element: <ProtectedLayout />,
    children: [
      {
        path: 'product',
        element: <Product />
      }
    ]
  }
])
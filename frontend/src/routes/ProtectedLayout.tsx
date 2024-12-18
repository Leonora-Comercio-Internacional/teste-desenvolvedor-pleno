import { useEffect } from 'react';
import { Outlet, useNavigate } from 'react-router-dom'

export function ProtectedLayout() {
  const navigate = useNavigate();

  const token = localStorage.getItem("authToken");

  useEffect(() => {
    if (!token) {
      navigate("/auth/signIn");
    }

  }, [token, navigate]);

  return token ? <Outlet /> : null;
}

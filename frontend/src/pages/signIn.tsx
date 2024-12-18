import { useForm } from "react-hook-form";
import imgLogo from "../assets/logo.png"
import { api } from "../lib/axios";
import { useNavigate } from "react-router-dom";

interface SignInFormData {
  username: string;
  password: string;
}

export function SignIn() {
  const navigate = useNavigate();

  const { handleSubmit, register, formState } = useForm<SignInFormData>();

  const onSubmit = async (data: SignInFormData) => {
    try {
      const response = await api.post("Auth/SignIn", data);
      const { token } = response.data;

      localStorage.setItem("authToken", token);

      navigate("/app/product", { replace: true });
    } catch {
      alert("Credenciais inválidas");
    }
  };

  return (
    <div className="container-fluid bg-dark text-white min-vh-100 d-flex flex-column">
      <header className="text-center py-4">
        <img src={imgLogo} alt="Logo" className="logo mb-3" />
      </header>
      <main className="container d-flex justify-content-center align-items-center flex-grow-1">
        <div className="card bg-light text-dark p-4" style={{ maxWidth: "400px" }}>
          <h2 className="text-center mb-4">Grupo Leonora</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="mb-3">
              <label htmlFor="username" className="form-label">
                Nome
              </label>
              <input
                type="text"
                className={`form-control ${formState.errors.username ? "is-invalid" : ""}`}
                id="username"
                {...register("username", { required: "Username is required" })}
              />
              {formState.errors.username && (
                <div className="invalid-feedback">{formState.errors.username.message}</div>
              )}
            </div>
            <div className="mb-3">
              <label htmlFor="password" className="form-label">
                Senha
              </label>
              <input
                type="password"
                className={`form-control ${formState.errors.password ? "is-invalid" : ""}`}
                id="password"
                {...register("password", { required: "Password is required" })}
              />
              {formState.errors.password && (
                <div className="invalid-feedback">{formState.errors.password.message}</div>
              )}
            </div>
            <button type="submit" className="btn btn-primary w-100 mb-3">
              Entrar
            </button>
          </form>
          <button
            className="btn btn-link w-100"
            onClick={() => navigate("/auth/signUp")}
          >
            Não tem uma conta? Cadastre-se aqui
          </button>
        </div>
      </main>

    </div>
  )
}
import { useForm } from "react-hook-form";
import imgLogo from "../assets/logo.png"
import { api } from "../lib/axios";
import { useNavigate } from "react-router-dom";
import { AuthFormData } from "../schemas/validationSchemas";

export const SignUp = () => {
  const navigate = useNavigate();

  const { handleSubmit, register, formState: { errors } } = useForm<AuthFormData>();

  const onSubmit = async (data: AuthFormData) => {
    try {
      await api.post("Auth/RegisterUser", data);

      alert("Registrado com sucesso");

      navigate("/auth/signIn", { replace: true });
    } catch {
      alert("Houve um erro ao tentar se registrar");
    }
  };

  return (
    <div className="container-fluid bg-dark text-white min-vh-100 d-flex flex-column">
      <header className="text-center py-4">
        <img src={imgLogo} alt="Logo" className="logo mb-3" />
      </header>
      <main className="container d-flex justify-content-center align-items-center flex-grow-1">
        <div className="card bg-light text-dark p-4" style={{ maxWidth: "400px" }}>
          <h2 className="text-center mb-4">Cadastro</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="mb-3">
              <label htmlFor="username" className="form-label">
                Nome
              </label>
              <input
                type="text"
                className={`form-control ${errors.username ? "is-invalid" : ""}`}
                id="username"
                {...register("username")}
              />
              {errors.username && (
                <div className="invalid-feedback">{errors.username.message}</div>
              )}
            </div>

            <div className="mb-3">
              <label htmlFor="password" className="form-label">
                Senha
              </label>
              <input
                type="password"
                className={`form-control ${errors.password ? "is-invalid" : ""}`}
                id="password"
                {...register("password")}
              />
              {errors.password && (
                <div className="invalid-feedback">{errors.password.message}</div>
              )}
            </div>

            <button type="submit" className="btn btn-primary w-100 mb-3">
              Criar Conta
            </button>
          </form>
          <button
            className="btn btn-link w-100"
            onClick={() => navigate("/auth/signIn")}
          >
            Já possui uma conta? Faça login aqui
          </button>
        </div>
      </main>

    </div>
  )
}
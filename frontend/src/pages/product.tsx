import { useEffect, useState } from "react"

import type { Product } from "../types/product"

import { ModalAddProduct } from "../components/modalAddProduct"

import imgLogo from "../assets/logo.png"
import { formatNumberCurrency } from "../utils/format"
import { ModalEditProduct } from "../components/modalEditProduct"
import { ModalDeleteProduct } from "../components/modalDeleteProduct"
import { api } from "../lib/axios"
import { useNavigate } from "react-router-dom"

enum Category {
  Eletrônicos = 1,
  Móveis = 2,
  Alimentos = 3,
  Roupas = 4,
  Eletrodomésticos = 5,
}

const Supplier = {
  1: "Comercial Santos LTDA",
  2: "Distribuidora Nova Era",
  3: "Alimentos União S.A.",
  4: "Móveis e Design LTDA",
  5: "Tech Solutions Inc."
};

export function Product() {
  const [products, setProducts] = useState<Product[]>([])
  const [activeModal, setActiveModal] = useState<"add" | "edit" | "delete" | null>(null);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await api.get<Product[]>('Product/GetAllProduct')
        setProducts(response.data)
      } catch {
        alert("Houve um erro ao buscar produtos");
      }
    }

    fetchData()
  }, [])

  const deleteProduct = async (id: number) => {
    try {
      await api.delete(`Product/DeleteProductById/${id}`)
      setProducts(products.filter((product) => product.id !== id))
    } catch {
      alert("Houve um erro ao excluir produto");
    }
  }

  const logout = () => {
    localStorage.removeItem("authToken");

    navigate('/auth/signIn');
  }

  return (
    <div className="container-fluid bg-dark text-white min-vh-100 d-flex flex-column">
      <header className="text-center py-4">
        <nav className="d-flex justify-content-end">
          <button
            onClick={logout}
            className="btn btn-danger mb-2"
          >
            Sair
          </button>
        </nav>
        <img src={imgLogo} alt="Logo" className="logo mb-3" />

      </header>

      <div className="container-fluid flex-grow-1 d-flex flex-column">
        <div className="row justify-content-center">
          <div className="col-md-10 col-lg-10 py-4">
            <button
              className="btn btn-primary mb-4"
              onClick={() => setActiveModal("add")}
            >
              Adicionar Produto
            </button>

            <legend className="mb-4">Produtos</legend>
            <table className="table table-dark table-striped text-center">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Nome</th>
                  <th>Preço</th>
                  <th>Descrição</th>
                  <th>Categoria</th>
                  <th>Fornecedor(es)</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {products.map((product) => (
                  <tr key={product.id}>
                    <td>{product.id}</td>
                    <td>{product.name}</td>
                    <td>{formatNumberCurrency(product.price)}</td>
                    <td>{product.description}</td>
                    <td>{Category[product.categoryId]}</td>
                    <td>{Array.isArray(product.supplierId) && product.supplierId.length > 0
                      ? product.supplierId
                        .map((supplierId) => {
                          const supplierName = Supplier[supplierId as unknown as keyof typeof Supplier];
                          return supplierName || `Unknown Supplier (ID: ${supplierId})`;
                        })
                        .join(", ")
                      : "Sem Fornecedor"}</td>
                    <td>
                      <button className="btn btn-primary btn-sm me-2" onClick={() => {
                        setSelectedProduct(product);
                        setActiveModal("edit");
                      }}>
                        Editar
                      </button>
                      <button className="btn btn-danger btn-sm" onClick={() => {
                        setSelectedProduct(product);
                        setActiveModal("delete");
                      }}>Excluir</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      <footer className="text-center py-3">
        <p className="mb-0">© 2024 Grupo Leonora</p>
      </footer>

      {activeModal === "add" && (
        <ModalAddProduct
          isModalOpen={true}
          setIsModalOpen={() => setActiveModal(null)}
        />
      )}

      {activeModal === "edit" && selectedProduct && (
        <ModalEditProduct
          product={selectedProduct}
          isModalOpen={true}
          setIsModalOpen={() => setActiveModal(null)}
        />
      )}

      {activeModal === "delete" && selectedProduct && (
        <ModalDeleteProduct
          product={selectedProduct}
          isModalOpen={true}
          setIsModalOpen={() => setActiveModal(null)}
          onDelete={deleteProduct}
        />
      )}
    </div>
  )
}
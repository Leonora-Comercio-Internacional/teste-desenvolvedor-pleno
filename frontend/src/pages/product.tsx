import { useEffect, useState } from "react"

import type { Product } from "../types/product"

import { ModalAddProduct } from "../components/modalAddProduct"

import imgLogo from "../assets/logo.png"
import { formatNumberCurrency } from "../utils/format"
import { ModalEditProduct } from "../components/modalEditProduct"
import { ModalDeleteProduct } from "../components/modalDeleteProduct"
import { api } from "../lib/axios"

enum Category {
  Eletrônicos = 1,
  Móveis = 2,
  Alimentos = 3,
  Roupas = 4,
  Eletrodomésticos = 5,
}

export function Product() {
  const [products, setProducts] = useState<Product[]>([])
  const [activeModal, setActiveModal] = useState<"add" | "edit" | "delete" | null>(null);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await api.get<Product[]>('/Product')
        setProducts(response.data)
      } catch (error) {
        console.log(error)
      }
    }

    fetchData()
  }, [])

  const deleteProduct = async (id: number) => {
    try {
      await api.delete(`/Product/${id}`)
      setProducts(products.filter((product) => product.id !== id))
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <div className="bg-dark text-white min-vh-100 d-flex flex-column">
      <header className="text-center py-4">
        <img src={imgLogo} alt="Logo" className="logo mb-3" />
        <h1 className="fw-bold">Grupo Leonora</h1>
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
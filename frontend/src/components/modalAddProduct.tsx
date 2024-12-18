import { useForm } from "react-hook-form";

import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";

import { api } from "../lib/axios";

import { zodResolver } from "@hookform/resolvers/zod";

import { productSchema, ProductFormData } from "../schemas/validationSchemas";

interface ModalAddProductProps {
  isModalOpen: boolean;
  setIsModalOpen: (value: boolean) => void;
}

export function ModalAddProduct({ isModalOpen, setIsModalOpen }: ModalAddProductProps) {
  const { handleSubmit, register, formState: { errors } } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
  })

  const onSubmit = async (data: ProductFormData) => {
    try {
      if (data.supplierIds) {
        data.supplierIds = [parseInt(data.supplierIds.toString())];
      }

      if (data.categoryId) {
        data.categoryId = parseInt(data.categoryId.toString());
      }

      if (data.price) {
        data.price = parseFloat(data.price.toString().replace(',', '.'));
      }

      await api.post('Product/AddProduct', data)

      setIsModalOpen(false)

      window.location.reload();
    } catch {
      alert("Houve um erro ao adicionar produto");
    }
  }

  return (
    <Modal isOpen={isModalOpen}>
      <ModalHeader>Adicionar Produto</ModalHeader>
      <ModalBody>
        <form id="productForm" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label>Nome</label>
            <input {...register('name')} type="text" className="form-control" />
            {errors.name && <span className="text-danger">{errors.name.message}</span>}
          </div>

          <div className="form-group">
            <label>Preço</label>
            <input {...register('price')} type="text" className="form-control" />
            {errors.price && <span className="text-danger">{errors.price.message}</span>}
          </div>

          <div className="form-group">
            <label>Descrição</label>
            <input {...register('description')} type="text" className="form-control" />
            {errors.description && <span className="text-danger">{errors.description.message}</span>}
          </div>

          <div className="form-group">
            <label>Categoria</label>
            <select {...register('categoryId')} className="form-control" aria-placeholder="Selecione uma Categoria">
              <option value=""></option>
              <option value="1">Eletrônicos</option>
              <option value="2">Móveis</option>
              <option value="3">Alimentos</option>
              <option value="4">Roupas</option>
              <option value="5">Eletrodomésticos</option>
            </select>
            {errors.categoryId && <span className="text-danger">{errors.categoryId.message}</span>}
          </div>

          <div className="form-group">
            <label>Fornecedores</label>
            <select {...register('supplierIds')} className="form-control" aria-placeholder="Selecione uma Fornecedor">
              <option value=""></option>
              <option value="1">Fornecedor A</option>
              <option value="2">Fornecedor B</option>
              <option value="3">Fornecedor C</option>
            </select>
            {errors.supplierIds && <span className="text-danger">{errors.supplierIds.message}</span>}
          </div>
        </form>
      </ModalBody>
      <ModalFooter>
        <button type="submit" className="btn btn-primary" form="productForm">Salvar</button>
        <button className="btn btn-danger" onClick={() => setIsModalOpen(false)}>Cancelar</button>
      </ModalFooter>
    </Modal>
  )
}
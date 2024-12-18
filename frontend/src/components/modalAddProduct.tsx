import { useForm } from "react-hook-form";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import type { ProductRequest } from "../types/product";
import { api } from "../lib/axios";

interface ModalAddProductProps {
  isModalOpen: boolean;
  setIsModalOpen: (value: boolean) => void;
}

export function ModalAddProduct({ isModalOpen, setIsModalOpen }: ModalAddProductProps) {
  const { handleSubmit, register, formState } = useForm<ProductRequest>()

  const onSubmit = async (data: ProductRequest) => {
    try {
      if (data.supplierIds) {
        data.supplierIds = [parseInt(data.supplierIds.toString())];
      }

      if (data.categoryId) {
        data.categoryId = parseInt(data.categoryId.toString());
      }

      if (data.price) {
        data.price = parseFloat(data.price.toString());
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
        <form id="productForm" onSubmit={handleSubmit(onSubmit)} className="form-group">
          <label>Nome</label>
          <input {...register('name')} type="text" className="form-control" />
          {formState.errors.name && <span>{formState.errors.name.message}</span>}
          <label>Preço</label>
          <input {...register('price')} type="number" className="form-control" />
          {formState.errors.price && <span>{formState.errors.price.message}</span>}

          <label>Descrição</label>
          <input {...register('description')} type="text" className="form-control" />
          {formState.errors.description && <span>{formState.errors.description.message}</span>}

          <label>Categoria</label>
          <select {...register('categoryId')} className="form-control">
            <option value="1">Eletrônicos</option>
            <option value="2">Móveis</option>
            <option value="3">Alimentos</option>
            <option value="4">Roupas</option>
            <option value="5">Eletrodomésticos</option>
          </select>
          {formState.errors.categoryId && <span>{formState.errors.categoryId.message}</span>}

          <label>Fornecedores</label>
          <select {...register('supplierIds')} className="form-control">
            <option value=""></option>
            <option value="1">Fornecedor A</option>
            <option value="2">Fornecedor B</option>
            <option value="3">Fornecedor C</option>
          </select>

        </form>
      </ModalBody>
      <ModalFooter>
        <button type="submit" className="btn btn-primary" form="productForm">Salvar</button>
        <button className="btn btn-danger" onClick={() => setIsModalOpen(false)}>Cancelar</button>
      </ModalFooter>
    </Modal>
  )
}
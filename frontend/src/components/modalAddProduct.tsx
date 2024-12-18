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
      await api.post('/Product', data)

      setIsModalOpen(false)
    } catch (error) {
      console.log(error)
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

        </form>
      </ModalBody>
      <ModalFooter>
        <button type="submit" className="btn btn-primary" form="productForm">Salvar</button>
        <button className="btn btn-danger" onClick={() => setIsModalOpen(false)}>Cancelar</button>
      </ModalFooter>
    </Modal>
  )
}
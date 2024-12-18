import { useForm } from "react-hook-form";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import type { Product, ProductRequest } from "../types/product";
import { useEffect } from "react";
import { api } from "../lib/axios";

interface ModalEditProductProps {
  product: Product | null;
  isModalOpen: boolean;
  setIsModalOpen: (value: boolean) => void;
}
export function ModalEditProduct({ product, isModalOpen, setIsModalOpen }: ModalEditProductProps) {
  const { handleSubmit, register, formState, setValue, reset } = useForm<ProductRequest>({
    defaultValues: {
      name: product?.name,
      price: product?.price,
      description: product?.description,
      categoryId: product?.categoryId
    }
  });

  useEffect(() => {
    if (product) {
      setValue("name", product.name);
      setValue("price", product.price);
      setValue("description", product.description);
      setValue("categoryId", product.categoryId);
    } else {
      reset();
    }
  }, [product, setValue, reset]);

  const onSubmit = async (data: ProductRequest) => {
    try {
      const id = product?.id;

      await api.put(`/Product/${id}`, data);

      setIsModalOpen(false)
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <Modal isOpen={isModalOpen}>
      <ModalHeader>Editar Produto</ModalHeader>
      <ModalBody>
        <form id="productEditForm" onSubmit={handleSubmit(onSubmit)} className="form-group">
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
        <button type="submit" className="btn btn-primary" form="productEditForm">Salvar</button>
        <button className="btn btn-danger" onClick={() => setIsModalOpen(false)}>Cancelar</button>
      </ModalFooter>
    </Modal>
  )
}
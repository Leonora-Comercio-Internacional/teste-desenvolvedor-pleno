import { Controller, useForm } from "react-hook-form";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import type { Product } from "../types/product";
import { useEffect } from "react";
import { api } from "../lib/axios";

import { zodResolver } from "@hookform/resolvers/zod";

import { productSchema, ProductFormData } from "../schemas/validationSchemas";

interface ModalEditProductProps {
  product: Product | null;
  isModalOpen: boolean;
  setIsModalOpen: (value: boolean) => void;
}
export function ModalEditProduct({ product, isModalOpen, setIsModalOpen }: ModalEditProductProps) {
  const { handleSubmit, register, formState: { errors }, setValue, reset, control } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
    defaultValues: {
      name: product?.name ?? "",
      price: product?.price ?? 0,
      description: product?.description ?? undefined,
      categoryId: product?.categoryId ?? 0,
      supplierIds: product?.supplierId !== undefined ? product.supplierId : []
    }
  });

  useEffect(() => {
    if (product) {
      setValue("name", product.name || "");
      setValue("price", product.price || 0);
      setValue("description", product.description || "");
      setValue("categoryId", product.categoryId || 0);
      setValue("supplierIds", product.supplierId || []);
    } else {
      reset();
    }
  }, [product, setValue, reset]);

  const onSubmit = async (data: ProductFormData) => {
    try {
      const id = product?.id;

      if (data.supplierIds) {
        data.supplierIds = [parseInt(data.supplierIds.toString())];
      }

      if (data.categoryId) {
        data.categoryId = parseInt(data.categoryId.toString());
      }

      if (data.price) {
        data.price = parseFloat(data.price.toString());
      }

      await api.put(`Product/UpdateProductById/${id}`, data);

      setIsModalOpen(false)

      window.location.reload();
    } catch {
      alert("Houve um erro ao editar produto");
    }
  }

  return (
    <Modal isOpen={isModalOpen}>
      <ModalHeader>Editar Produto</ModalHeader>
      <ModalBody>
        <form id="productEditForm" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label>Nome</label>
            <input {...register('name')} type="text" className="form-control" />
            {errors.name && <span className="text-danger">{errors.name.message}</span>}
          </div>

          <div className="form-group">
            <label>Preço</label>

            <Controller name="price" control={control} render={({ field }) => (
              <input {...field}
                type="text"
                className="form-control"
                value={field.value.toLocaleString('pt-br',
                  { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
              />
            )} />
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
        <button type="submit" className="btn btn-primary" form="productEditForm">Salvar</button>
        <button className="btn btn-danger" onClick={() => setIsModalOpen(false)}>Cancelar</button>
      </ModalFooter>
    </Modal>
  )
}
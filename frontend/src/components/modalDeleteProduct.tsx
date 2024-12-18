import { Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import type { Product } from "../types/product";

interface ModalDeleteProductProps {
  product: Product | null;
  isModalOpen: boolean;
  setIsModalOpen: (value: boolean) => void;
  onDelete: (id: number) => void;
}

export function ModalDeleteProduct({
  product,
  isModalOpen,
  setIsModalOpen,
  onDelete,
}: ModalDeleteProductProps) {

  return (
    <Modal isOpen={isModalOpen}>
      <ModalHeader>Excluir Produto</ModalHeader>
      <ModalBody>
        {product ? (
          <p>
            Tem certeza de que deseja excluir o produto{" "}
            <strong>{product.name}</strong>?
          </p>
        ) : (
          <p>Produto não encontrado.</p>
        )}
      </ModalBody>
      <ModalFooter>
        <button
          className="btn btn-danger"
          onClick={() => {
            if (product) {
              onDelete(product.id);
            }
            setIsModalOpen(false);
          }}
        >
          Excluir
        </button>
        <button
          className="btn btn-secondary"
          onClick={() => setIsModalOpen(false)}
        >
          Cancelar
        </button>
      </ModalFooter>
    </Modal>
  );
}

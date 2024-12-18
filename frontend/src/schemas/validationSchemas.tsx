import { z } from "zod";

export const authSchema = z.object({
  username: z
    .string()
    .nonempty("O nome de usuário é obrigatório.")
    .min(3, "O nome de usuário deve ter pelo menos 3 caracteres.")
    .max(20, "O nome de usuário pode ter no máximo 20 caracteres."),
  password: z
    .string()
    .nonempty("A senha é obrigatória.")
    .min(6, "A senha deve ter pelo menos 6 caracteres.")
    .max(30, "A senha pode ter no máximo 30 caracteres."),
});

export type AuthFormData = z.infer<typeof authSchema>;

const ptBrNumber = z.preprocess((val) => {
  if (typeof val === "string") {
    const parsed = parseFloat(val.replace(".", "").replace(",", "."));
    return isNaN(parsed) ? null : parsed;
  }
  return val;
}, z.number().nullable().refine((val) => val !== null, "O preço é obrigatório."));

export const productSchema = z.object({
  name: z.string().nonempty("O nome do produto é obrigatório."),
  price: ptBrNumber,
  description: z.string().optional(),
  categoryId: z
    .string()
    .nonempty("A categoria é obrigatória.")
    .transform((val) => parseInt(val)),
  supplierIds: z
    .string()
    .nonempty("O fornecedor é obrigatório.")
    .transform((val) => [parseInt(val)]),
});

export type ProductFormData = z.infer<typeof productSchema>;
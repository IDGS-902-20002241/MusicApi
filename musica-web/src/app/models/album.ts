import { Genero } from './genero';

export interface Album {
  id: number;
  titulo: string;
  artista: string;
  anioLanzamiento: number;
  disquera: string;
  descripcion?: string;
  precio?: number;
  imagen?: string;
  generoId: number;
  genero?: Genero;
}

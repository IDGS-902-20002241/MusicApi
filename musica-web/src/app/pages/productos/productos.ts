import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MusicaService } from '../../services/musica';
import { Album } from '../../models/album';
import { Genero } from '../../models/genero';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './productos.html',
  styleUrl: './productos.css'
})
export class Productos implements OnInit {

  albumes: Album[] = [];
  albumesFiltrados: Album[] = [];
  generos: Genero[] = [];

  textoBusqueda = '';
  generoSeleccionado = '';

  cargando = false;
  error = '';
  mensaje = '';

  editando = false;
  idEditando = 0;

  nuevoGenero: Genero = {
    id: 0,
    nombre: ''
  };

  albumFormulario: Album = {
    id: 0,
    titulo: '',
    artista: '',
    anioLanzamiento: new Date().getFullYear(),
    disquera: '',
    descripcion: '',
    precio: 0,
    imagen: '',
    generoId: 0
  };

  constructor(private musicaService: MusicaService) { }

  ngOnInit(): void {
    this.cargarGeneros();
    this.cargarAlbumes();
  }

  cargarGeneros(): void {
    this.musicaService.obtenerGeneros().subscribe({
      next: (data) => {
        this.generos = data;
      },
      error: (error) => {
        console.error(error);
        this.error = 'No se pudieron cargar los géneros.';
      }
    });
  }

  cargarAlbumes(): void {
  this.cargando = true;
  this.error = '';

  this.musicaService.obtenerAlbumes().subscribe({
    next: (data) => {
      this.albumes = data;
      this.aplicarFiltrosActuales();
      this.cargando = false;
    },
    error: (error) => {
      console.error(error);
      this.error = 'No se pudieron cargar los álbumes desde la API.';
      this.cargando = false;
    }
  });
}
aplicarFiltrosActuales(): void {
  const texto = this.textoBusqueda.toLowerCase().trim();

  this.albumesFiltrados = this.albumes.filter(album => {
    const coincideTexto =
      album.titulo.toLowerCase().includes(texto) ||
      album.artista.toLowerCase().includes(texto);

    const coincideGenero =
      this.generoSeleccionado === '' ||
      album.generoId === Number(this.generoSeleccionado);

    return coincideTexto && coincideGenero;
  });
}
  guardarGenero(): void {
    if (this.nuevoGenero.nombre.trim() === '') {
      this.error = 'Escribe el nombre del género.';
      return;
    }

    this.musicaService.crearGenero(this.nuevoGenero).subscribe({
      next: () => {
        this.mensaje = 'Género agregado correctamente.';
        this.error = '';
        this.nuevoGenero = { id: 0, nombre: '' };
        this.cargarGeneros();
      },
      error: (error) => {
        console.error(error);
        this.error = 'No se pudo guardar el género.';
      }
    });
  }

  guardarAlbum(): void {
    if (
      this.albumFormulario.titulo.trim() === '' ||
      this.albumFormulario.artista.trim() === '' ||
      this.albumFormulario.generoId === 0
    ) {
      this.error = 'Completa al menos título, artista y género.';
      return;
    }

    if (this.editando) {
      this.actualizarAlbum();
    } else {
      this.crearAlbum();
    }
  }

 crearAlbum(): void {
  this.musicaService.crearAlbum(this.albumFormulario).subscribe({
    next: () => {
      this.mensaje = 'Álbum agregado correctamente.';
      this.error = '';

      this.limpiarFormulario();
      this.textoBusqueda = '';
      this.generoSeleccionado = '';

      this.cargarAlbumes();
    },
    error: (error) => {
      console.error(error);
      this.error = 'No se pudo guardar el álbum.';
    }
  });
}

  actualizarAlbum(): void {
  this.musicaService.actualizarAlbum(this.idEditando, this.albumFormulario).subscribe({
    next: () => {
      this.mensaje = 'Álbum actualizado correctamente.';
      this.error = '';

      this.limpiarFormulario();
      this.textoBusqueda = '';
      this.generoSeleccionado = '';

      this.cargarAlbumes();
    },
    error: (error) => {
      console.error(error);
      this.error = 'No se pudo actualizar el álbum.';
    }
  });
}

  editarAlbum(album: Album): void {
    this.editando = true;
    this.idEditando = album.id;

    this.albumFormulario = {
      id: album.id,
      titulo: album.titulo,
      artista: album.artista,
      anioLanzamiento: album.anioLanzamiento,
      disquera: album.disquera,
      descripcion: album.descripcion || '',
      precio: album.precio || 0,
      imagen: album.imagen || '',
      generoId: album.generoId
    };

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  eliminarAlbum(id: number): void {
  const confirmar = confirm('¿Seguro que quieres eliminar este álbum?');

  if (!confirmar) {
    return;
  }

  this.musicaService.eliminarAlbum(id).subscribe({
    next: () => {
      this.mensaje = 'Álbum eliminado correctamente.';
      this.error = '';

      this.textoBusqueda = '';
      this.generoSeleccionado = '';

      this.cargarAlbumes();
    },
    error: (error) => {
      console.error(error);
      this.error = 'No se pudo eliminar el álbum.';
    }
  });
}

 filtrarProductos(): void {
  this.aplicarFiltrosActuales();
 }
  limpiarFiltros(): void {
  this.textoBusqueda = '';
  this.generoSeleccionado = '';
  this.albumesFiltrados = [...this.albumes];
}

  limpiarFormulario(): void {
    this.editando = false;
    this.idEditando = 0;

    this.albumFormulario = {
      id: 0,
      titulo: '',
      artista: '',
      anioLanzamiento: new Date().getFullYear(),
      disquera: '',
      descripcion: '',
      precio: 0,
      imagen: '',
      generoId: 0
    };
  }
}

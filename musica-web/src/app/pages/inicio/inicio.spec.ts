import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './inicio.html',
  styleUrl: './inicio.css'
})
export class Inicio {

  productosMuestra = [
    {
      titulo: 'Midnight Vibes',
      artista: 'Luna Beats',
      genero: 'Pop',
      precio: 299,
      imagen: 'https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?auto=format&fit=crop&w=800&q=80'
    },
    {
      titulo: 'Rock Legends',
      artista: 'The Thunder Band',
      genero: 'Rock',
      precio: 349,
      imagen: 'https://images.unsplash.com/photo-1516280440614-37939bbacd81?auto=format&fit=crop&w=800&q=80'
    },
    {
      titulo: 'Retro Vinyl',
      artista: 'Classic Soul',
      genero: 'Jazz',
      precio: 279,
      imagen: 'https://images.unsplash.com/photo-1520166012956-add9ba0835cb?auto=format&fit=crop&w=800&q=80'
    }
  ];

}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Album } from '../models/album';
import { Genero } from '../models/genero';

@Injectable({
  providedIn: 'root'
})
export class MusicaService {

  private apiUrl = 'http://localhost:5120';

  constructor(private http: HttpClient) { }

  obtenerAlbumes(): Observable<Album[]> {
    return this.http.get<Album[]>(`${this.apiUrl}/api/albumes`);
  }

  obtenerGeneros(): Observable<Genero[]> {
    return this.http.get<Genero[]>(`${this.apiUrl}/api/generos`);
  }

  crearAlbum(album: Album): Observable<Album> {
    return this.http.post<Album>(`${this.apiUrl}/api/albumes`, album);
  }

  actualizarAlbum(id: number, album: Album): Observable<Album> {
    return this.http.put<Album>(`${this.apiUrl}/api/albumes/${id}`, album);
  }

  eliminarAlbum(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/albumes/${id}`);
  }

  crearGenero(genero: Genero): Observable<Genero> {
    return this.http.post<Genero>(`${this.apiUrl}/api/generos`, genero);
  }

  actualizarGenero(id: number, genero: Genero): Observable<Genero> {
    return this.http.put<Genero>(`${this.apiUrl}/api/generos/${id}`, genero);
  }

  eliminarGenero(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/generos/${id}`);
  }
}

import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  areas_trabajo: any[] = [];
  username: string = "";
  url_backend: string = "http://localhost:5100/";

  constructor( private http: HttpClient) {
  }

  ngOnInit(): void {
    this.iniciar_areas();
    Swal.fire("SweetAlert2 is working!");
  }

  login() {
    const data: any = {
      username: '',
      password: ''
    };
    this.http.post(this.url_backend + 'api/Auth/login', data).subscribe(
      (response) => {
        console.log('Respuesta del servidor:', response);
      },
      (error) => {
        console.error('Error en la solicitud:', error);
      }
    );
  }

  iniciar_areas() {
    this.areas_trabajo = [];
    const series: string[] = ['a', 'b', 'c', 'd'];
    const numeros: number[] = [1, 2, 3, 4, 5];
    let area: any[] = [];
    for (const serie of series) {
      let grupo: any[] = [];
      for (const numero of numeros) {
        grupo.push({
          puesto: serie + numero,
          asignado: "",
        });
      }
      area.push(grupo);
    }
    this.areas_trabajo.push(area);
  }
}

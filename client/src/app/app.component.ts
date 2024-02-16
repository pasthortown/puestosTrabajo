import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  areas_trabajo: any[] = [];
  identificadores_area_trabajo: any[] = [];
  url_backend: string = "http://localhost:5100/";
  equipo_trabajo: any = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.iniciar_areas();
  }

  get_personas() {
    this.equipo_trabajo = [];
    this.http.get(this.url_backend + 'api/Person').subscribe(
      (response) => {
        this.equipo_trabajo = response;
        this.get_agendas_semana_actual();
        //this.buildAsignaciones();
      },
      (error) => {
        console.error('Error en la solicitud:', error);
      }
    );
  }

  get_person_by_id(id: Number) {
    let toReturn = '';
    this.equipo_trabajo.forEach((element: any) => {
      console.log(element.id);
      if (element.id == id) {
        toReturn = element.nombre;
      }
    });
    return toReturn;
  }

  get_agendas_semana_actual() {
    let semana_actual = this.obtenerItemSemanaActual();
    this.http.get(this.url_backend + 'api/Schedule/fecha/' + semana_actual.fechaDesde).subscribe(
      (agenda_semana_actual: any) => {
        this.areas_trabajo.forEach((area: any) => {
          for (let index = 0; index < area.length; index++) {
            const grupo: any[] = area[index];
            grupo.forEach(mesa => {
              agenda_semana_actual.forEach((agenda: any) => {
                if (agenda.workspace == mesa.puesto) {
                  mesa.asignado = this.get_person_by_id(agenda.person_id);
                }
              });
            });
          }
        });
      },
      (error) => {
        console.error('Error en la solicitud:', error);
      }
    );
  }

  obtenerItemSemanaActual(): any | null {
    const semanasDelAnio: any[] = this.generarSemanasDelAnio();
    const hoy = new Date();
    for (let i = 0; i < semanasDelAnio.length; i++) {
        const fechaDesdeSemana = new Date(semanasDelAnio[i].fechaDesde);
        const fechaHastaSemana = new Date(semanasDelAnio[i].fechaHasta);
        if (hoy >= fechaDesdeSemana && hoy <= fechaHastaSemana) {
            return semanasDelAnio[i];
        }
    }
    return null;
  }

  iniciar_areas() {
    this.areas_trabajo = [];
    this.identificadores_area_trabajo = [];
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
        this.identificadores_area_trabajo.push(serie + numero);
      }
      area.push(grupo);
    }
    this.areas_trabajo.push(area);
    this.get_personas();
  }

  generarSemanasDelAnio() {
    const semanasDelAnio: any[] = [];
    const primerLunes = new Date(new Date().getFullYear(), 0, 1);
    primerLunes.setDate(primerLunes.getDate() + ((1 - primerLunes.getDay() + 7) % 7));
    let fechaDesde = new Date(primerLunes);
    while (fechaDesde.getFullYear() === primerLunes.getFullYear()) {
        const fechaHasta = new Date(fechaDesde);
        fechaHasta.setDate(fechaHasta.getDate() + 6);
        const semana: any = {
            fechaDesde: fechaDesde.toISOString().slice(0, 10),
            fechaHasta: fechaHasta.toISOString().slice(0, 10)
        };
        semanasDelAnio.push(semana);
        fechaDesde.setDate(fechaDesde.getDate() + 7);
    }
    return semanasDelAnio;
  }

  moverPrimerElementoAlFinal(arreglo: any[]): any[] {
    const primerElemento = arreglo.shift();
    arreglo.push(primerElemento);
    return arreglo;
  }

  buildAsignaciones() {
    let id = 1;
    let semanas_anio: any[] = this.generarSemanasDelAnio();
    semanas_anio.forEach((semana: any) => {
      this.identificadores_area_trabajo.forEach((id_area_trabajo: any) => {
        const persona = this.equipo_trabajo.shift();
        this.equipo_trabajo.push(persona);
        let agenda: any = {
          "id": id,
          "workspace": id_area_trabajo,
          "start_date": semana.fechaDesde,
          "end_date": semana.fechaHasta,
          "person_id": persona.id
        };
        this.http.post(this.url_backend + 'api/Schedule', agenda).subscribe(
          (response) => {
            console.log(agenda);
          },
          (error) => {
            console.error('Error en la solicitud:', error);
          }
        );
        id = id + 1;
      });
      this.equipo_trabajo = this.moverPrimerElementoAlFinal(this.equipo_trabajo);
    });
  }
}

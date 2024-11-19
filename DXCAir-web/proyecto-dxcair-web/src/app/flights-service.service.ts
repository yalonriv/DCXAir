import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FlightFilterDTO } from './DTOs/flight.filter.dto';
import { Flight } from './DTOs/flight';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlightsService {

  // URL de la API .NET donde está expuesto el servicio
  private apiUrl = 'https://localhost:7057/flights/filterByOneWay';  // Cambia esta URL según corresponda

   // Inyectar HttpClient para hacer solicitudes HTTP
   constructor(private http: HttpClient) { }
 
   // Ejemplo de método para obtener los vuelos
    // Método para hacer el POST con el filtro
  searchFlightOneWay2(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<any>(this.apiUrl, filter, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  searchFlightOneWay(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<Flight[]>(this.apiUrl, filter, {headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    })});
  }
}

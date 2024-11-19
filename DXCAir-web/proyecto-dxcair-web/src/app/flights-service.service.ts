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


  // URL de la API .NET donde está expuesto el servicio
  private apiUrl2 = 'https://localhost:7057/flights/filterByRoundTrip';  // Cambia esta URL según corresponda

   // Inyectar HttpClient para hacer solicitudes HTTP
   constructor(private http: HttpClient) { }

  searchFlightOneWay(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<Flight[]>(this.apiUrl, filter, {headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    })});
  }

  searchFlightRoundTrip(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<any>(this.apiUrl2, filter, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }
}

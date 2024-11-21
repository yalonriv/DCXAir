import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FlightFilterDTO } from './DTOs/flight.filter.dto';
import { Flight } from './DTOs/flight';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
/**
 * Clase de servicios encargada de administrar la información de los vuelos
 */
export class FlightsService {

  // URL de la API .NET donde están los servicios
  private apiUrl = 'https://localhost:7057/flights/';  


   // Inyectar HttpClient para hacer solicitudes HTTP
   constructor(private http: HttpClient) { }

  
  /**
   * Método encargado de consumir el servicio que consulta los vuelos 
   * de solo ida
   * @param filter dto que contiene la información del origen, destino y tipo de moneda para 
   * consultar los vuelos
   * @returns lista de tipo Flight con la información de los vuelos consultados o 
   * un error si ocurre uno durante el consumo del servicio.
   */
  searchFlightOneWay(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<Flight[]>(this.apiUrl.concat('getFlightOneWay'), filter, {headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    })}).pipe(catchError(error => {
      // Enviar el error para manejarlo en el componente
      return throwError(() => new Error(error.error));
    }));
    
  }

   /**
   * Método encargado de consumir el servicio que consulta los vuelos 
   * de ida y vuelta
   * @param filter dto que contiene la información del origen, destino y tipo de moneda para 
   * consultar los vuelos
   * @returns lista de tipo Flight con la información de los vuelos consultados o 
   * un error si ocurre uno durante el consumo del servicio.
   */
  searchFlightRoundTrip(filter: FlightFilterDTO): Observable<Flight[]> {
    return this.http.post<any>(this.apiUrl.concat('getFlightsRoundTrip'), filter, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(catchError(error => {
      // Enviar el error para manejarlo en el componente
      return throwError(() => new Error(error.error));
    }));;
  }

   /**
    * Método encargado de consumir el servicio que consulta los origenes de vuelos disponibles
    */
  searchFlightOrigins(): Observable<string[]>{
    return this.http.get<string[]>(this.apiUrl.concat('getOrigins'), {headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    })}).pipe(catchError(error => {
      // Enviar el error para manejarlo en el componente
      return throwError(() => new Error(error.error));
    }));;
  }

  /**
    * Método encargado de consumir el servicio que consulta los destinos de vuelos disponibles
    */
  searchFlightDestinations(): Observable<string[]>{
    return this.http.get<string[]>(this.apiUrl.concat('getDestinations'), {headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': '*/*'
    })}).pipe(catchError(error => {
      // Enviar el error para manejarlo en el componente
      return throwError(() => new Error(error.error));
    }));;
  }

  
}

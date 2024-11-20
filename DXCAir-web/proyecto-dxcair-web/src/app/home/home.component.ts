import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FlightFilterDTO } from '../DTOs/flight.filter.dto';
import { FlightsService } from '../flights-service.service';
import { Flight } from '../DTOs/flight';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  providers: [FlightsService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  constructor(private flightService: FlightsService){}

  username = 'yamile';
  isLoggedIn = true;
  selectedFlightType = "";
  flights: Flight[] = [];  // Para almacenar los vuelos filtrados
  public coins: string[] = ['USD', 'EUR', 'COP', 'SOL'];
  public origins: string[];
  public destinations: string[];
  public flightFilterDTO;
  


  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    this.flightFilterDTO = new FlightFilterDTO();
    this.searchOriginFlights();
    this.searchDestinationFlights();
  }
  
  public searchFlight(){
    if(this.selectedFlightType === "one-way"){
      this.searchFlightOneWay();
    }
    else{
      this.searchFlightByRoundTrip();
    }
  }
  
  public searchFlightOneWay(){
    this.flightService.searchFlightOneWay(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;  // Aquí se almacenan los vuelos obtenidos
        console.log('Filtered flights:', this.flights);
      },
      (error) => {
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  public searchFlightByRoundTrip(){
    this.flightService.searchFlightRoundTrip(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;
      },
      (error) => {
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  private searchOriginFlights(){
    this.flightService.searchFlightOrigins().subscribe(
      (response) => {
        this.origins = response;
      },
      (error) => {
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  private searchDestinationFlights(){
    this.flightService.searchFlightDestinations().subscribe(
      (response) => {
        this.destinations = response;
      },
      (error) => {
        console.error('Error al obtener vuelos:', error);
      }
    );
  }
  
  
}

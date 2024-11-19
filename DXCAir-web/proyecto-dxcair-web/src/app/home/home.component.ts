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
  public origins: string[] = ['MZL', 'PEI', 'BOG', 'JFK', 'BCN', 'MAD'];
  public destinations: string[] = ['MZL', 'PEI', 'BOG', 'JFK', 'BCN', 'MAD'];
  public flightFilterDTO;
  


  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    this.flightFilterDTO = new FlightFilterDTO();
    this.flightFilterDTO.origin= 'MZL';
    this.flightFilterDTO.destination = 'PEI';
    this.flightFilterDTO.coinToConvert = 'USD';
    this.searchFlightOneWay();
  }
  
  public searchFlightOneWay(){
    this.flightService.searchFlightOneWay(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;  // Aquí se almacenan los vuelos obtenidos
        console.log('Filtered flights:', this.flights[0].price);
      },
      (error) => {
        console.error('Error al obtener vuelos:', error);
      }
    );
  }
  
}

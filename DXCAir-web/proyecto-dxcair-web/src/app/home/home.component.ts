import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FlightFilterDTO } from '../DTOs/flight.filter.dto';
import { FlightsService } from '../flights-service.service';
import { Flight } from '../DTOs/flight';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from '../error-dialog/error-dialog.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  providers: [FlightsService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  constructor(private flightService: FlightsService, private dialog: MatDialog) { }

  selectedFlightType = "";
  flights: Flight[] = [];  // Para almacenar los vuelos filtrados
  public coins: string[] = ['USD', 'EUR', 'GBP', 'MXN'];
  public origins: string[];
  public destinations: string[];
  public flightFilterDTO;
  public filteredOrigins: string[];
  public filteredDestinations: string[];
  public isSelectedOrigin: boolean = false;
  public formIsFilledOut: boolean;

  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    this.flightFilterDTO = new FlightFilterDTO();
    this.searchOriginFlights();
    this.searchDestinationFlights();
    this.formIsFilledOut = true;
    this.selectedFlightType = 'one-way';
  }

  public searchFlight() {
    this.validateForm();
    this.flights = [];
    if(this.formIsFilledOut === true){
      if (this.selectedFlightType === "one-way") {
        this.searchFlightOneWay();
      }
      else {
        this.searchFlightByRoundTrip();
      }
    }

    
  }

  public searchFlightOneWay() {
    this.flightService.searchFlightOneWay(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;  // Aquí se almacenan los vuelos obtenidos
        console.log('Filtered flights:', this.flights);
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  public searchFlightByRoundTrip() {
    this.flightService.searchFlightRoundTrip(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  private searchOriginFlights() {
    this.flightService.searchFlightOrigins().subscribe(
      (response) => {
        this.origins = response;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  private searchDestinationFlights() {
    this.flightService.searchFlightDestinations().subscribe(
      (response) => {
        this.destinations = response;
        this.filteredDestinations = this.destinations;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  // Método que actualiza las opciones filtradas
  updateFilteredOptions() {
    this.isSelectedOrigin = true;
    this.destinations = this.filteredDestinations;
    this.destinations = this.destinations.filter(item => item !== this.flightFilterDTO.origin);


  }

  showErrorModal(errorMessage: string) {
    this.dialog.open(ErrorDialogComponent, {
      data: errorMessage
    });
  }

  private validateForm() {
    if(this.flightFilterDTO.origin===undefined || this.flightFilterDTO.origin === '' || this.flightFilterDTO.origin === null){
      this.formIsFilledOut = false;
    }
    else{
      this.formIsFilledOut = true;
    }
    if(this.flightFilterDTO.destination===undefined || this.flightFilterDTO.destination === '' || this.flightFilterDTO.destination === null){
      this.formIsFilledOut = false;
    }
    else{
      this.formIsFilledOut = true;
    }
    if(this.flightFilterDTO.coinToConvert===undefined || this.flightFilterDTO.coinToConvert === '' || this.flightFilterDTO.coinToConvert === null){
      this.formIsFilledOut = false;
    }
    else{
      this.formIsFilledOut = true;
    }

  }

}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FlightFilterDTO } from '../DTOs/flight.filter.dto';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  username = 'yamile';
  isLoggedIn = true;

  options = ["Opt 1", "Opt 2", "Opt 3"];
  selectedValue = "";

  public coins: string[] = ['USD', 'EUR', 'COP', 'SOL'];
  public origins: string[] = ['MZL', 'PEI', 'BOG', 'JFK', 'BCN', 'MAD'];
  public destinations: string[] = ['MZL', 'PEI', 'BOG', 'JFK', 'BCN', 'MAD'];

  public flightFilterDTO = new FlightFilterDTO();
  
}

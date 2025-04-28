import { useState } from 'react'
import { Movie } from './types'
import './App.css'

function App() {
  const [searchQuery, setSearchQuery] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [movies, setMovies] = useState<Movie[]>([])
  const apiServer = import.meta.env.VITE_CHAT_API;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    
    try {
      const response = await fetch(`${apiServer}/movies?query=${encodeURIComponent(searchQuery)}`, {
        method: 'GET',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        },
      })
      const data = await response.json()
      setMovies(data)
    } catch (error) {
      console.error('Error fetching data:', error)
      setMovies([])
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <>
      <h1 className="title">Movies semantic search</h1>
      <div className="search-container">
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            placeholder="Enter your search query"
            disabled={isLoading}
          />
          <button type="submit" disabled={isLoading}>
            {isLoading ? 'Searching...' : 'Search'}
          </button>
        </form>
      </div>

      <div className="movies-grid">
        {movies.map((movie, index) => (
          <div key={index} className="movie-card">
            <img src={movie.imageUrl} alt={movie.title} className="movie-image" />
            <div className="movie-info">
              <h2>{movie.title}</h2>
              <p className="year">Released: {movie.releasedYear}</p>
              <p className="overview">{movie.overview}</p>
            </div>
          </div>
        ))}
      </div>
    </>
  )
}

export default App

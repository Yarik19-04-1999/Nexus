export function Spinner({ className = '' }: { className?: string }) {
  return (
    <div
      className={`w-5 h-5 border-2 border-gray-200 border-t-gray-600 rounded-full animate-spin ${className}`}
    />
  )
}

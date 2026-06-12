interface InviteCardProps {
  message: string
  description?: string
  children: React.ReactNode
}

export function InviteCard({ message, description, children }: InviteCardProps) {
  return (
    <div className="flex flex-col items-center gap-6 text-center">
      <h1 className="text-2xl font-bold text-gray-800 leading-snug max-w-sm">{message}</h1>
      {description && (
        <p className="text-gray-500 text-sm max-w-xs leading-relaxed">{description}</p>
      )}
      {children}
    </div>
  )
}

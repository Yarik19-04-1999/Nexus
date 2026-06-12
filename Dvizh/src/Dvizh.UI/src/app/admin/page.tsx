import { InvitesTable } from '@/components/admin/InvitesTable'

export default function AdminPage() {
  return (
    <main className="min-h-screen bg-gray-50 p-4 sm:p-8">
      <div className="max-w-4xl mx-auto">
        <InvitesTable />
      </div>
    </main>
  )
}
